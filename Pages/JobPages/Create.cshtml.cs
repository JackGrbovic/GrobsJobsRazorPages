using FluentValidation;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.JobPages
{
    public class CreateModel : PageModel
    {
        public string CreateJobErrorMessage { get; set; }

        private GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext;

        [BindProperty]
        public Job Job { get; set; }

        [BindProperty]
        public string JobType { get; set; }

        private IdentityUser? LoggedInUser { get; set; }

        private UserHandler UserHandler { get; set; }

        private IValidator<Job> JobValidator { get; set; }

        public CreateModel(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, UserHandler userGetter, IValidator<Job> jobValidator)
        {
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            UserHandler = userGetter;
            JobValidator = jobValidator;
        }

        public IActionResult OnGet(string createJobErrorMessage){
            if (!User.Identity.IsAuthenticated){
                return RedirectToPage("/UserPages/Login");
            }

            if (createJobErrorMessage != null){
                CreateJobErrorMessage = createJobErrorMessage;
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            LoggedInUser = UserHandler.ReturnFirstPartyUser(User);
            Job.DateTimePosted = DateTime.Now;
            Job.JobPosterNormalizedUserName = LoggedInUser.NormalizedUserName;
            Job.JobPosterId = LoggedInUser.Id;
            if(JobType == "Help Needed"){
                Job.JobType = JobType;
            }
            else if (JobType == "Here To Help"){
                Job.JobType = JobType;
            }

            var validationResult = await JobValidator.ValidateAsync(Job);

            if (validationResult.IsValid)
            {
                await GrobsJobsRazorPagesDbContext.Jobs.AddAsync(Job);
                await GrobsJobsRazorPagesDbContext.SaveChangesAsync();
                return RedirectToPage("/Index");
            }
            else{
                return RedirectToPage("/JobPages/Create", new { createJobErrorMessage = "An error occured while posting the job. Please try again." });
            }
        }
    }
}
