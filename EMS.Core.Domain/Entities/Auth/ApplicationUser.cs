using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities.Auth
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? ProfilePicture { get; set; }

        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public EmployeeEntity Employee { get; set; } = new EmployeeEntity();
    }
}
