using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities
{
    [Table("Project")]
    public class ProjectEntity: BaseEntity
    {
        public string Client { get; set; }
        public string Project { get; set; }
    }
}
