using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;

namespace EMS.Core.Domain.Services
{
    public interface IUserManagementService
    {
        Task<ValidationResult> AddAsync(Registration request);
    }
}
