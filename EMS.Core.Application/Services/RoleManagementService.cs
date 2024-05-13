using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Extensions;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EMS.Core.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleManagementService: IRoleManagementService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleManagementService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="logger"></param>
        public RoleManagementService(RoleManager<ApplicationRole> roleManager,
            ILogger<RoleManagementService> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<ValidationResult> AddAsync(string role)
        {
            var result = new ValidationResult();

            try
            {
                (bool roleExists, result.ValidationMessages) = await _roleManager.TryAddRole(role);
                return result;
            }
            catch (Exception ex)
            {
                result.ValidationMessages.Add("An error occurred while creating new user. Please contact support.");
                _logger.LogError("An error occurred while creating new user. Please see exception: " + ex.Message);
                return result;
            }
        }
    }
}
