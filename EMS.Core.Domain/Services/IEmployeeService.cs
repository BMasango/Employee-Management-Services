using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using System.Security.Claims;

namespace EMS.Core.Domain.Services
{
    public interface IEmployeeService
    {
        Task<ValidationResult<Employee>> GetByIdAsync(Guid id);
        Task<ValidationResult> UpdateAsync(Employee request, ClaimsPrincipal claimsPrincipal);
        Task<ValidationResult<bool>> DeleteByIdAsync(Guid id);
        Task<ValidationResult<List<Employee>>> GetEmployeesByManager(ClaimsPrincipal claimsPrincipal);
    }
}
