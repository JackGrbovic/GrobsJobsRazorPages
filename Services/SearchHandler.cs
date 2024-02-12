using GrobsJobsRazorPages.Data;
using Job = GrobsJobsRazorPages.Model.Job;

namespace GrobsJobsRazorPages.CustomServices
{
    public class SearchHandler
    {
        public List<Job> SearchResults = new List<Job>();

        public GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        public SearchHandler(GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext)
        {
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
        }

        public List<Job> GetJobFromSearchQueryString(string queryString)
        {
            return GrobsJobsRazorPagesDbContext.Jobs.Where(
                x => x.Name.Contains(queryString) || 
                x.Description.Contains(queryString) || 
                x.JobPosterNormalizedUserName.Contains(queryString))
                .ToList();
        }
    }
}
