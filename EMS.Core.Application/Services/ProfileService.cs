using AutoMapper;
using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using EMS.Core.Domain.Repositories;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EMS.Core.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(ILogger<ProfileService> logger,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ValidationResult<Employee>> ViewMyProfile(ClaimsPrincipal claimsPrincipal)
        {
            var result = new ValidationResult<Employee>();
            try
            {
                var signedInUser = await _userManager.GetUserAsync(claimsPrincipal);
                if (signedInUser == null)
                {
                    result.ValidationMessages.Add("Could not load signed in user profile.");
                    return result;
                }

                var entity = await _employeeRepository.GetByIdAsync(signedInUser.EmployeeId);
                if (entity == null)
                {
                    result.ValidationMessages.Add($"Could not find user with reference {signedInUser.EmployeeId}.");
                    return result;
                }

                result.Data = _mapper.Map<Employee>(entity);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to get personal details. Please see exception: {ex.Message}");
                result.ValidationMessages.Add($"An error occurred while trying to get personal details. Please see exception: {ex.Message}");
                return result;
            }
        }

        public async Task<ValidationResult> UploadImage(string image, ClaimsPrincipal claimsPrincipal)
        {
            var result = new ValidationResult();
            try
            {
                var signedInUser = await _userManager.GetUserAsync(claimsPrincipal);
                if (signedInUser == null)
                {
                    result.ValidationMessages.Add("Could not load signed in user profile.");
                    return result;
                }

                signedInUser.ProfilePicture = image;
                var identityResult = await _userManager.UpdateAsync(signedInUser);
                if(!identityResult.Succeeded)
                {
                    result.ValidationMessages.AddRange(identityResult.Errors.Select(x => x.Description));
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to save user's image. Please see exception: {ex.Message}");
                result.ValidationMessages.Add($"An error occurred while trying to save user's image. Please see exception: {ex.Message}");
                return result;
            }
        }
    }
}
