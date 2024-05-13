using AutoMapper;
using EMS.Core.Application.Services;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    /// <summary>
    /// My Profile Controller
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileService"></param>
        /// <param name="mapper"></param>
        public ProfileController(IProfileService profileService,
            IMapper mapper)
        {
            _profileService = profileService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the signed in user profile
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserDetails")]
        
        public async Task<IActionResult> GetUserDetails()
        {
            var user = HttpContext.User;
            var result = await _profileService.ViewMyProfile(user);
            if (!result.IsValid) 
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }


            var employeeDetails = _mapper.Map<EmployeeDetails>(result.Data);
            employeeDetails.IsFormEnabled = false;
            employeeDetails.SaveHidden = true;
            return PartialView("employeeDetails", employeeDetails);
        }

        /// <summary>
        /// Save new profile image to user profile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var user = HttpContext.User;
            var result = new ValidationResult();
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string baseString = Convert.ToBase64String(fileBytes);
                    result = await _profileService.UploadImage(baseString, user);
                }
            }

            if (!result.IsValid)
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }
            return View();
        }
    }
}
