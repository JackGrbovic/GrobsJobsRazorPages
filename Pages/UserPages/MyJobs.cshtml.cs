using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.CustomServices.PartialViewControllers;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class MyJobsModel : PageModel
    {
        public int PageNumber { get; set; }

        public string Route = "/UserPages/MyJobs";
        public _JobListController _JobListController { get; set; }
        private GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        public IdentityUser LoggedInUser { get; set; }

        private UserHandler UserHandler { get; set; }

        public List<Job> Jobs { get; set; }

        public bool UserIsAuthenticated;

        public MyJobsModel(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, UserHandler userHandler) 
        { 
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            UserHandler = userHandler;
        }

        public void OnGet(int pageNumber)
        {
            if (pageNumber == 0){
                PageNumber = 1;
            }
            else PageNumber = pageNumber;
            
            if (User.Identity.IsAuthenticated == false)
            {
                UserIsAuthenticated = false;
            }

            else 
            {
                UserIsAuthenticated = true;
                LoggedInUser = UserHandler.ReturnFirstPartyUser(User);
                Jobs = GrobsJobsRazorPagesDbContext.Jobs.Where(j => j.JobPosterId == LoggedInUser.Id).OrderByDescending(j => j.DateTimePosted).ToList();
                _JobListController = new _JobListController(Jobs, PageNumber, Route);
            }
        }
    }
}
