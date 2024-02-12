using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using GrobsJobsRazorPages.Model;
using GrobsJobsRazorPages.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class UserProfileModel : PageModel
    {
        public IdentityUser LoggedInUser { get; set; } 

        public UserHandler UserHandler { get; set; }

        public UserProfileModel(UserHandler userHandler)
        {
            UserHandler = userHandler;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated){
                LoggedInUser = UserHandler.ReturnFirstPartyUser(User);
                return Page();
            } 
            return RedirectToPage("/Error", new { errorMessage = "User not signed in." });
        }
    }
}
