using EMS.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Core.Domain.Entities
{
    [Table("Address")]
    public class AddressEntity: BaseEntity
    {
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string CitySuburb { get; set; }
        public Province Province { get; set; }
        public string PostalCode { get; set; }
    }
}
