using EMS.Core.Domain.Models.Request;

namespace EMS.Core.Domain.Models
{
    public class EmployeeDetails: Employee
    {
        public bool IsFormEnabled { get; set; }
        public bool SaveHidden { get; set; }
    }
}
