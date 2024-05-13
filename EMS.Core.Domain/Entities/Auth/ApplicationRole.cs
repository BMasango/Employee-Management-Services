using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities.Auth
{
    [Table("ApplicationRole")]
    public class ApplicationRole: IdentityRole<Guid>
    {
        public ApplicationRole():base()
        {
            
        }

        public ApplicationRole(string role):base(role)
        {
            
        }
    }
}
