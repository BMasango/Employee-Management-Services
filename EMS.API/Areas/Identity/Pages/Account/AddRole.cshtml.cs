using EMS.Core.Domain.Entities.Auth;
using EMS.Core.Domain.Models.Request;
using EMS.Core.Domain.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EMS.API.Areas.Identity.Pages.Account
{
    public class AddRoleModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AddRoleModel> _logger;
        private readonly IRoleManagementService _roleManagementService;

        public AddRoleModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<AddRoleModel> logger,
            IRoleManagementService roleManagementService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _roleManagementService = roleManagementService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string RoleName { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _roleManagementService.AddAsync(Input.RoleName);
                if (result.IsValid)
                {
                    _logger.LogInformation("New role type added.");

                    return Page();
                }
                foreach (var error in result.ValidationMessages)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            return Page();
        }
    }
}
