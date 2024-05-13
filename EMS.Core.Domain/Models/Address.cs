using EMS.Core.Domain.Enums;

namespace EMS.Core.Domain.Models
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string CitySuburb { get; set; }
        public Province Province { get; set; }
        public string PostalCode { get; set; }
    }
}
