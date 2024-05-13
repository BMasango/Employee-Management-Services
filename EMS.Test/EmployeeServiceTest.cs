using AutoMapper;
using EMS.Core.Application.Services;
using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using EMS.Core.Domain.Repositories;
using EMS.Core.Domain.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace EMS.Test.Services
{
    public class EmployeeServiceTest
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeService _mockedEmployeeService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeServiceTest()
        {
            _mockedEmployeeService = A.Fake<IEmployeeService>();
            _logger = A.Fake<ILogger<EmployeeService>>();
            _employeeRepository = A.Fake<IEmployeeRepository>();
            _mapper = A.Fake<IMapper>();
            _userManager = A.Fake<UserManager<ApplicationUser>>();
        }

        [Fact]
        public async Task GetEmployee_NonExisting()
        {
            var fakeId = Guid.NewGuid();
            var result = await _mockedEmployeeService.GetByIdAsync(fakeId);
            result.Should().BeOfType<Task<ValidationResult<Employee>>>();
            Assert.Equal(false, result.IsValid);
        }

        [Fact]
        public async Task GetEmployee_Existing()
        {
            var fakeId = new Guid("e2f325dd-1fb5-4d0c-b819-08dc72b1a1ca");
            var result = await _mockedEmployeeService.GetByIdAsync(fakeId);
            result.Should().BeOfType<Task<ValidationResult<Employee>>>();
            Assert.Equal(true, result.IsValid);
        }

        [Fact]
        public async Task DeleteEmployee_NonExisting()
        {
            var fakeId = Guid.NewGuid();
            var result = await _mockedEmployeeService.DeleteByIdAsync(fakeId);
            result.Should().BeOfType<Task<ValidationResult<bool>>>();
            Assert.Equal(false, result.IsValid);
        }

        [Fact]
        public async Task DeleteEmployee_Existing()
        {
            var fakeId = new Guid("78c094d5-866b-4595-6d54-08dc72ba1442");
            var result = await _mockedEmployeeService.DeleteByIdAsync(fakeId);
            result.Should().BeOfType<Task<ValidationResult<Employee>>>();

            //Wont always be true since after first delete record wont be available
            //Ideally should be adding and deleting the newly created
            Assert.Equal(true, result.IsValid);
        }
    }
}
