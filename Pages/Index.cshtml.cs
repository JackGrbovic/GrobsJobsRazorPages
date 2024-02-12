using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.CustomServices.PartialViewControllers;
using GrobsJobsRazorPages.Model;
using GrobsJobsRazorPages.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GrobsJobsRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public bool isLoggedIn;

        string Route = "/Index";
        private int PageNumber { get; set; }

        public string postJobRoute;

        public _JobListController JobListController { get; set; }

        private readonly GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext;
        
        private UserHandler UserHandler { get ; set; } 
 
        public List<Job> Jobs { get; set; }

        public IndexModel(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, UserHandler userHandler)
        {
            UserHandler = userHandler;
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
        }

        public async Task OnGetAsync(int pageNumber)
        {
            if (pageNumber == 0){
                PageNumber = 1;
            }
            else PageNumber = pageNumber;

            var jobsFromServer = await GrobsJobsRazorPagesDbContext.Jobs.OrderByDescending(x => x.DateTimePosted).ToListAsync();


            isLoggedIn = UserHandler.IsUserLoggedIn(User);
            if (isLoggedIn){
                postJobRoute = "/JobPages/Create";
            }
            else postJobRoute = "/UserPages/Login";

            Jobs = new List<Job>(jobsFromServer);
            JobListController = new _JobListController(Jobs, PageNumber, Route);
        }
    }
}