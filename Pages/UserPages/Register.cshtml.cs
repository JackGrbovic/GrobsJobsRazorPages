using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using GrobsJobsRazorPages.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Register RegisterViewModel {  get; set; }

        public UserManager<IdentityUser> UserManager { get; }
        
        public SignInManager<IdentityUser> SignInManager { get; }

        private UserHandler UserHandler { get; set; }

        private AuthDbContext AuthDbContext { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UserHandler userHandler, AuthDbContext authDbContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            UserHandler = userHandler;
            AuthDbContext = authDbContext;
        }

        public async Task<IActionResult> OnPostAsync() 
        { 
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    Email = RegisterViewModel.Email,
                    NormalizedEmail = RegisterViewModel.Email,
                    UserName = RegisterViewModel.DisplayName,
                    EmailConfirmed = true
                };

                bool IsUserNameUnique;
                bool IsEmailUnique;

                var doesEmailAlreadyExist = AuthDbContext.Users.Where(x => x.Email == user.Email);

                IsUserNameUnique = UserHandler.IsRegistrationStringValid(user.UserName);
                IsEmailUnique = UserHandler.IsRegistrationStringValid(user.Email);

                string userNameTakenError = "Sorry, but the UserName you have provided is already taken. Please choose a different UserName";
                string emailTakenError = "Sorry, but the Email you have provided is already taken. Please choose a different Email";

                if (IsUserNameUnique == false)
                {
                    string errorMessage = userNameTakenError;
                    return RedirectToPage("/Error", new { errorMessage });
                }

                if (IsEmailUnique == false)
                {
                    string errorMessage = emailTakenError;
                    return RedirectToPage("/Error", new { errorMessage });
                }

                var result = await UserManager.CreateAsync(user, RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
