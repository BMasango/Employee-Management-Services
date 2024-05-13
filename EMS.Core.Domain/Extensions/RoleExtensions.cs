using EMS.Core.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace EMS.Core.Domain.Extensions
{
    public static class RoleExtensions
    {
        public static async Task<Tuple<bool, List<string>>> TryAddRole(this RoleManager<ApplicationRole> roleManager, string role)
        {
            var errorMsg = new List<string>();
            var hasRole = await roleManager.RoleExistsAsync(role);
            if (hasRole)
                return Tuple.Create(true, errorMsg);

            var newRole = new ApplicationRole(role);
            var identityResult = await roleManager.CreateAsync(newRole);
            if (!identityResult.Succeeded)
            {
                errorMsg.AddRange(identityResult.Errors.Select(x => x.Description));
                return Tuple.Create(false, errorMsg);
            }

            return Tuple.Create(true, errorMsg);
        }
    }
}
