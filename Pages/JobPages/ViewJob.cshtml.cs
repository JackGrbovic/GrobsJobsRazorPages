using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.JobPages
{
    public class ViewJobModel : PageModel
    {
        private readonly GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext;

        [BindProperty]
        public Job Job { get; set; }

        private AuthDbContext AuthDbContext { get; set; }

        public IdentityUser LoggedInUser { get; set; }

        private UserHandler UserGetter {  get; set; }

        public bool AuthorizedToEdit { get; set; }

        public bool AuthenticatedToMessageJobPoster { get; set; }

        public ViewJobModel(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, UserHandler userGetter, AuthDbContext authDbContext)
        {
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            UserGetter = userGetter;
            AuthDbContext = authDbContext;
        }

        public void OnGet(int id)
        {
            Job = GrobsJobsRazorPagesDbContext.Jobs.Find(id);
            
            IdentityUser user = UserGetter.ReturnFirstPartyUser(User);

            if (AuthDbContext.Users.Contains(user))
            {
                LoggedInUser = UserGetter.ReturnFirstPartyUser(User);

                if (Job.JobPosterId == LoggedInUser.Id)
                {
                    AuthorizedToEdit = true;
                    AuthenticatedToMessageJobPoster = false;
                }
                else if (Job.JobPosterId != LoggedInUser.Id)
                {
                    AuthorizedToEdit = false;
                    AuthenticatedToMessageJobPoster = true;
                }
            }
            else
            {
                AuthorizedToEdit = false;
                AuthenticatedToMessageJobPoster = false;
            }
        }
    }
}
