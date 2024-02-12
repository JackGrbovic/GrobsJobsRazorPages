using FluentValidation;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.JobPages
{
    public class EditJobModel : PageModel
    {
        public string EditJobErrorMessage { get; set; }

        [BindProperty]
        public string JobType { get; set; }
        
        private readonly GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext;

        [BindProperty]
        public Job? Job { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsDelete { get; set; }

        IValidator<Job> JobValidator { get; set; }

        IdentityUser? LoggedInUser { get; set; }

        UserHandler UserHandler { get; set; }

        public EditJobModel(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, IValidator<Job> jobValidator, UserHandler userHandler)
        {
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            JobValidator = jobValidator;
            UserHandler = userHandler;
        }

        public void OnGet(int id, string editJobErrorMessage)
        {
            Job = GrobsJobsRazorPagesDbContext.Jobs.Find(id);
            if(Job != null && Job.JobType == "Help Needed"){
                JobType = "Help Needed";
            }
            else if (Job != null && Job.JobType == "Here To Help"){
                JobType = "Here To Help";
            }
            EditJobErrorMessage = editJobErrorMessage;
        }

        public async Task<IActionResult> OnPost(string handler, int jobId)
        {
            //declare the job again and use its id lower down to avoid fuckery
            //do intro panel in index
            LoggedInUser = UserHandler.ReturnFirstPartyUser(User);

            if (Job != null && LoggedInUser != null){
                Job.Id = jobId;
                Job.DateTimePosted = DateTime.Now;
                if(LoggedInUser.NormalizedUserName != null) Job.JobPosterNormalizedUserName = LoggedInUser.NormalizedUserName;
                Job.JobPosterId = LoggedInUser.Id;
                if(JobType == "Help Needed"){
                    Job.JobType = JobType;
                }
                else if (JobType == "Here To Help"){
                    Job.JobType = JobType;
                }

                if (handler == "Update")
                {
                    IsUpdate = true;
                }
                else if (handler == "Delete")
                {
                    IsDelete = true;
                }

                var validationResult = JobValidator.Validate(Job);

                if (IsUpdate && validationResult.IsValid)
                {
                    GrobsJobsRazorPagesDbContext.Jobs.Update(Job);
                    await GrobsJobsRazorPagesDbContext.SaveChangesAsync();
                    return RedirectToPage("/JobPages/ViewJob", new { id = Job.Id });
                }
                else if (IsDelete)
                {
                    var jobFromDb = GrobsJobsRazorPagesDbContext.Jobs.Where(j => j.Id == Job.Id);
                    if (jobFromDb != null) {
                        GrobsJobsRazorPagesDbContext.Jobs.Remove(Job);
                        await GrobsJobsRazorPagesDbContext.SaveChangesAsync();
                        return RedirectToPage("/Index");
                    }
                }
            }
            
            return RedirectToPage("/JobPages/EditJob", new { editJobErrorMessage = "An error occured while editing the job. Please try again." });
        }
    }
}
