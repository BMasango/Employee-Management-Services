using AutoMapper;
using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using EMS.Core.Domain.Repositories;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EMS.Core.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(ILogger<EmployeeService> logger,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ValidationResult<Employee>> GetByIdAsync(Guid id)
        {
            var result = new ValidationResult<Employee>();
            try
            {
                var entity = await _employeeRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogError($"No user found with id {id}");
                    result.ValidationMessages.Add($"No user found with id {id}");
                    return result;
                }

                result.Data = _mapper.Map<Employee>(entity);
                var user = await _userManager.FindByEmailAsync(result.Data.Email);
                if (user == null)
                {
                    _logger.LogError($"Could not load user with id {id}");
                    result.ValidationMessages.Add($"Could not load user with id {id}");
                    return result;
                }

                var roles = await _userManager.GetRolesAsync(user);
                result.Data.Role = roles?.FirstOrDefault() ?? "";
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to get employee {id}. Please see exception: {ex.Message}");
                result.ValidationMessages.Add($"An error occurred while trying to get employee {id}. Please see exception: {ex.Message}");
                return result;
            }
        }

        public async Task<ValidationResult<bool>> DeleteByIdAsync(Guid id)
        {
            var result = new ValidationResult<bool>();
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);
                if (employee == null)
                {
                    _logger.LogError($"No user found with id {id}");
                    result.ValidationMessages.Add($"No user found with id {id}");
                    return result;
                }

                var user = await _userManager.FindByEmailAsync(employee.Email);
                if (user == null)
                {
                    _logger.LogError($"Could not load user with id {id}");
                    result.ValidationMessages.Add($"Could not load user with id {id}");
                    return result;
                }

                var identityResult = await _userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    var errors = identityResult.Errors.Select(x => x.Description).ToList();
                    _logger.LogError(string.Join(';', errors));
                    result.ValidationMessages.AddRange(errors);
                    return result;
                }

                result.Data = await _employeeRepository.DeleteByIdAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to delete employee with id {id}. Please see exception: {ex.Message}");
                result.ValidationMessages.Add($"An error occurred while trying to delete employee with id {id}. Please see exception: {ex.Message}");
                return result;
            }
        }

        public async Task<ValidationResult> UpdateAsync(Employee request, ClaimsPrincipal claimsPrincipal)
        {
            var result = new ValidationResult();

            try
            {
                var signedInUser = await _userManager.GetUserAsync(claimsPrincipal);
                if (signedInUser.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogError("User cannot update their own profile.");
                    result.ValidationMessages.Add("User cannot update their own profile.");
                    return result;
                }

                var employee = _mapper.Map<EmployeeEntity>(request);
                await _employeeRepository.UpdateAsync(employee);
                return result;
            }
            catch (Exception ex)
            {
                result.ValidationMessages.Add("An error occurred while updating the employee. Please contact support.");
                _logger.LogError("An error occurred while updating the employee. Please see exception: " + ex.Message);
                return result;
            }
        }

        public async Task<ValidationResult<List<Employee>>> GetEmployeesByManager(ClaimsPrincipal claimsPrincipal)
        {
            var result = new ValidationResult<List<Employee>>();
            try
            {
                var signedInUser = await _userManager.GetUserAsync(claimsPrincipal);
                if (signedInUser == null)
                {
                    _logger.LogError("Could not load signed in user.");
                    result.ValidationMessages.Add("Could not load signed in user.");
                    return result;
                }

                var email = signedInUser.Email;
                var entities = await _employeeRepository.GetAllAsync(x => x.LineManagersEmail == email);
                result.Data = _mapper.Map<List<Employee>>(entities);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while trying to get employees. Please see exception: {ex.Message}");
                result.ValidationMessages.Add($"An error occurred while trying to get employees. Please see exception: {ex.Message}");
                return result;
            }
        }
    }
}
