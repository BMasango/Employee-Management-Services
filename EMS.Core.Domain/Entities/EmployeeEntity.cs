using EMS.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities
{
    [Table("Employee")]
    public class EmployeeEntity: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender? Gender { get; set; }
        public string Email { get; set; }
        public string LineManagersEmail { get; set; }

        public Guid? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public AddressEntity Address { get; set; }
    }
}
