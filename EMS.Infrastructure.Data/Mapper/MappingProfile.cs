using AutoMapper;
using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;

namespace EMS.Infrastructure.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Registration, ApplicationUser>()
                .ForMember(d => d.Email, s => s.MapFrom(o => o.Email))
                .ForMember(d => d.UserName, s => s.MapFrom(o => o.Email))
                .AfterMap((source, dest) =>
                {
                    dest.Employee.FirstName = source.FirstName;
                    dest.Employee.LastName = source.LastName;
                    dest.Employee.Email = source.Email;
                    dest.Employee.LineManagersEmail = source.LineManagersEmail;
                    dest.Employee.Age = source.Age;
                });

            CreateMap<Employee, EmployeeEntity>()
                .ForMember(d => d.LineManagersEmail, s => s.MapFrom(o => o.ManagersEmail))
                .ReverseMap()
                .ForMember(d => d.ManagersEmail, s => s.MapFrom(o => o.LineManagersEmail));

            CreateMap<Address, AddressEntity>()
                .ReverseMap();

            CreateMap<Employee, EmployeeDetails>();
        }
    }
}
