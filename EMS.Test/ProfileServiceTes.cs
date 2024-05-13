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
    public class ProfileServiceTest
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IProfileService _mockedProfileService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileServiceTest()
        {
            _mockedProfileService = A.Fake<IProfileService>();
            _logger = A.Fake<ILogger<ProfileService>>();
            _employeeRepository = A.Fake<IEmployeeRepository>();
            _mapper = A.Fake<IMapper>();
            _userManager = A.Fake<UserManager<ApplicationUser>>();
        }

        [Fact]
        public async Task GetLoggedInUserProfile()
        {
            
        }

        [Fact]
        public async Task ChangeProfilePicture()
        {
            
        }
    }
}
