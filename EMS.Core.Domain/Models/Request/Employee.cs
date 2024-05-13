using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Enums;

namespace EMS.Core.Domain.Models.Request
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string ManagersEmail { get; set; }
        public string Role { get; set; }
    }
}
