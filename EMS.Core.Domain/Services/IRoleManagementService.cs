using EMS.Core.Domain.Models;

namespace EMS.Core.Domain.Services
{
    public interface IRoleManagementService
    {
        Task<ValidationResult> AddAsync(string role);
    }
}
