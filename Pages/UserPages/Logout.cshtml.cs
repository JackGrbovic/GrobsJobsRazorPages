using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> SignInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public IActionResult OnGet(){
            if (User.Identity.IsAuthenticated){
                return Page();
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync() 
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage("/UserPages/Login");
        }

        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("/Index");
        }
    }
}
