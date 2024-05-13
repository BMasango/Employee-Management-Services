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
    public class RoleManagementServiceTest
    {
        private readonly ILogger<RoleManagementService> _logger;
        private readonly IRoleManagementService _mockedRoleManagementService;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleManagementServiceTest()
        {
            _mockedRoleManagementService = A.Fake<IRoleManagementService>();
            _logger = A.Fake<ILogger<RoleManagementService>>();
            _roleManager = A.Fake<RoleManager<ApplicationRole>>();
        }

        [Fact]
        public async Task Add_NewRole()
        {
            var result = await _mockedRoleManagementService.AddAsync("ENGINEER");
            result.Should().BeOfType<Task<ValidationResult>>();

            //Wont always give the desired result because once added it wont add again.
            Assert.Equal(true, result.IsValid);
        }

        [Fact]
        public async Task Add_ExistingRole()
        {
            var result = await _mockedRoleManagementService.AddAsync("ADMIN");
            result.Should().BeOfType<Task<ValidationResult>>();
            Assert.Equal(false, result.IsValid);
        }

    }
}
