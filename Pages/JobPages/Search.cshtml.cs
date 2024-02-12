using GrobsJobsRazorPages.CustomServices.PartialViewControllers;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrobsJobsRazorPages.Data;

namespace GrobsJobsRazorPages.Pages.JobPages
{
    [ValidateAntiForgeryToken]
    public class SearchModel : PageModel
    {
        public _JobListController _JobListController { get; set; }

        string Route = "/JobPages/Search";

        public int PageNumber { get; set; }

        private readonly SearchHandler SearchHandler;

        public bool noResults = false;

        public List<Job> JobResults = new List<Job>();

        public GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        public SearchModel(SearchHandler searchHandler, GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext)
        {
            SearchHandler = searchHandler;
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
        }

        public void OnGet(int pageNumber){
            var jobsFromServer = GrobsJobsRazorPagesDbContext.Jobs.OrderByDescending(x => x.DateTimePosted);
            JobResults = new List<Job>(jobsFromServer);
            _JobListController = new _JobListController(JobResults, pageNumber, Route);

            if (pageNumber == 0){
                PageNumber = 1;
            }
            else PageNumber = pageNumber;

            _JobListController = new _JobListController(JobResults, pageNumber, Route);

            if (!JobResults.Any())
            {
                noResults = true;
            }
        }

        public IActionResult OnPost(string queryString, int pageNumber)
        {
            JobResults = SearchHandler.GetJobFromSearchQueryString(queryString);

            if (pageNumber == 0){
                PageNumber = 1;
            }
            else PageNumber = pageNumber;

            _JobListController = new _JobListController(JobResults, 1, Route);


            if (JobResults.Any())
            {
                return Page();
            }
            else
            {
                noResults = true;
                return Page();
            }
        }
    }
}
