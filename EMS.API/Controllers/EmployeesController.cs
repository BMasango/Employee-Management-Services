using AutoMapper;
using EMS.Core.Domain.Entities;
using EMS.Core.Domain.Models;
using EMS.Core.Domain.Models.Request;
using EMS.Core.Domain.Repositories;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMS.API.Controllers
{
    /// <summary>
    /// Employees Controller
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = "MANAGER")]
    public class EmployeesController : Controller
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="employeeService"></param>
        public EmployeesController(ILogger<EmployeesController> logger,
            IEmployeeService employeeService,
            IMapper mapper)
        {
            _logger = logger;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Update existing employee
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateAsync")]
        public async Task<IActionResult> UpdateAsync(Employee request)
        {
            var user = HttpContext.User;
            var result = await _employeeService.UpdateAsync(request, user);
            if (!result.IsValid)
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }

            return await GetManagersEmployees();
        }

        /// <summary>
        /// Get's employees assigned to the signed in manager
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetManagersEmployees()
        {
            var user = HttpContext.User;
            var result = await _employeeService.GetEmployeesByManager(user);
            if (!result.IsValid)
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }

            return View("employees", result.Data);
        }

        /// <summary>
        /// Get's employees details by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("employee/{id}")]
        public async Task<IActionResult> Employee(Guid id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            if (!result.IsValid)
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }

            var employeeDetails = _mapper.Map<EmployeeDetails>(result.Data);
            employeeDetails.IsFormEnabled = true;
            employeeDetails.SaveHidden = false;
            return PartialView("employeeDetails", employeeDetails);
        }

        /// <summary>
        /// Delete's (soft delete) employees profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("deleteById")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            var result = await _employeeService.DeleteByIdAsync(id);
            if (!result.IsValid)
            {
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View("errorHandler", result);
            }

            return await GetManagersEmployees();
        }
    }
}
