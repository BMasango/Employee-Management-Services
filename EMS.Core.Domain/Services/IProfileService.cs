using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using System.Security.Claims;

namespace EMS.Core.Domain.Services
{
    public interface IProfileService
    {
        Task<ValidationResult<Employee>> ViewMyProfile(ClaimsPrincipal claimsPrincipal);
        Task<ValidationResult> UploadImage(string image, ClaimsPrincipal claimsPrincipal);
    }
}
