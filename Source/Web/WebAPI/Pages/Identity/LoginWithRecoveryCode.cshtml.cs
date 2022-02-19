using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebAPI.Pages.Identity
{
    [AllowAnonymous]
    public class LoginWithRecoveryCodeModel : PageModel
    {
        private SignInManager<ApplicationUser> signInManager;
        public LoginWithRecoveryCodeModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
        [BindProperty]
        public string RecoveryCode { get; set; }

        public LoginWithRecoveryCodeModel(string errorMessage)
        {
            if (errorMessage != null)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
            }
        }
        public async Task<ActionResult> OnPost()
        {
            var signInResult = await signInManager.TwoFactorRecoveryCodeSignInAsync(RecoveryCode);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(ReturnUrl);
            }
            return Redirect("/");
        }
    }
}
