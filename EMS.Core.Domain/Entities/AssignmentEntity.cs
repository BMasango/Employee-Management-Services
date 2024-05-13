using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities
{
    [Table("Assignment")]
    public class AssignmentEntity: BaseEntity
    {
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public EmployeeEntity Employee { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public ProjectEntity Project { get; set; }
    }
}
