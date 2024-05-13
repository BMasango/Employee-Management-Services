using AutoMapper;
using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Enums;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Extensions;
using EMS.Core.Domain.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using EMS.Core.Domain.Services;
using EMS.Core.Domain.Repositories;
using EMS.Core.Domain.Entities;

namespace EMS.Core.Application.Services
{
    public class UserManagementService: IUserManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<UserManagementService> _logger;
        private readonly IMapper _mapper;
        
        public UserManagementService(UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            ILogger<UserManagementService> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<ValidationResult> AddAsync(Registration request)
        {
            var result = new ValidationResult();

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    result.ValidationMessages.Add("Email already exists.");
                    return result;
                }
                
                var newUser = _mapper.Map<ApplicationUser>(request);
                var identityResult = await _userManager.CreateAsync(newUser, request.Password);
                if (!identityResult.Succeeded)
                {
                    result.ValidationMessages.AddRange(identityResult.Errors.Select(x => x.Description));
                    return result;
                }

                user = await _userManager.FindByEmailAsync(request.Email);
                var roleResult = await AddUserRoler(user, request.Role);
                if (roleResult != null && !roleResult.IsValid)
                    result.ValidationMessages.AddRange(roleResult.ValidationMessages);

                var manager = await _userManager.FindByEmailAsync(request.LineManagersEmail);
                if (manager == null)
                {
                    result.ValidationMessages.Add("Invalid manager provided.");
                    return result;
                }

                return result;

            }
            catch (Exception ex)
            {
                result.ValidationMessages.Add("An error occurred while creating new user. Please contact support.");
                _logger.LogError("An error occurred while creating new user. Please see exception: " + ex.Message);
                return result;
            }
        }

        public async Task<ValidationResult> AddUserRoler(ApplicationUser user, string role)
        {
            var result = new ValidationResult();

            (bool hasRole, result.ValidationMessages) = await _roleManager.TryAddRole(role.ToString());
            if (!hasRole)
                return result;

            //Add role for the user
            var userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Any(x => x == role.ToString()))
            {
                var identityResult = await _userManager.AddToRoleAsync(user, role.ToString());
                if (!identityResult.Succeeded)
                {
                    result.ValidationMessages.AddRange(identityResult.Errors.Select(x => x.Description));
                    return result;
                }
            }

            return result;
        }
    }
}
