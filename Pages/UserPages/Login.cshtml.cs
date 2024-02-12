using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> SignInManager;

        public string LoginErrorMessage { get; set; }

        [BindProperty]
        public Login LoginViewModel { get; set; }

        public LoginModel(CustomSignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public void OnGet(string loginErrorMessage){
            LoginErrorMessage = loginErrorMessage;
        }

        public async Task<IActionResult> OnPostAsync(/*string returnUrl = null*/)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await SignInManager.PasswordSignInAsync(LoginViewModel.Email, LoginViewModel.Password, LoginViewModel.RememberMe, false);
                if (identityResult.Succeeded)
                {
                        return RedirectToPage("/Index");
                }
                return RedirectToPage("/UserPages/Login", new { loginErrorMessage = "Email or Password was incorrect." });
            }
            return Page();
        }
    }
}
