using GrobsJobsRazorPages.Model;

namespace GrobsJobsRazorPages.CustomServices.PartialViewControllers
{
    public class _JobListController
    {
        public int PageNumber { get; set; }   

        public string Route { get; set; }

        public List<int> PaginationNumbers { get; set; }

        public List<Job> Jobs { get; set; }

        public int JobCountPerPage = 10;

        public int TotalCountOfJobs { get; set; }

        public _JobListController(List<Job> jobs, int pageNumber, string route){
            Jobs = jobs;
            PageNumber = pageNumber;
            Route = route;
            PaginationNumbers = GetPaginationNumbers(PageNumber, Jobs, JobCountPerPage);
            Jobs = GetJobsForPage(PageNumber, Jobs);
        }

         public List<int> GetPaginationNumbers(int currentPageNumber, List<Job> argJobs, int jobCountPerPage){
            List<int> numbersToReturn = new List<int>();

            //This sorts the array for the case of the page number being the penultimate or final page number
            int penultimatePageNumber = 0;
            int finalPageNumber = 0;

            //This determines if the number is divisible by jobCountPerPage, and if, when divided by jobCountPerPage, it is equal
            //To penultimateNumber or finalNumber
            for (int i = argJobs.Count; i > 0; i--){
                if (i % jobCountPerPage == 0) {
                    penultimatePageNumber = (i - jobCountPerPage) / jobCountPerPage;
                    finalPageNumber = i / jobCountPerPage;
                    break;
                }
            }

            //If pagenumber is equal to either of those, we create our int array with the select number in mind
            if (currentPageNumber == penultimatePageNumber){
                for (int i = penultimatePageNumber - 3; i < penultimatePageNumber + 1; i++){
                    numbersToReturn.Add(i);
                }
                return numbersToReturn;
            }
            else if (currentPageNumber == finalPageNumber && finalPageNumber != 1){
                for (int i = penultimatePageNumber - 4; i < finalPageNumber; i++){
                    numbersToReturn.Add(i);
                }
                return numbersToReturn;
            }

            //This sorts the array for the case of the page number being the first, second, or any other page number before the last two
            int subtractCurrantPageNumberBy;

            if (currentPageNumber == 1) subtractCurrantPageNumberBy = 0;
            else if (currentPageNumber == 2) subtractCurrantPageNumberBy = 1;
            else subtractCurrantPageNumberBy = 2;
            
            //This has a possibility to assign a value from 1 to 5, checking whether argJobs.Count is within the range of up to 5 pages
            //If it's not up to 5, then the number is cut short, as it will be caught by the Enumerable.Range method
            int firstNumberInArray = currentPageNumber - subtractCurrantPageNumberBy;
            int numberOfPages = 0;
            for (int i = 0; i < 5 * jobCountPerPage; i++){
                int lowerBand = i * jobCountPerPage;
                int upperBand = (i + 1) * jobCountPerPage;
                if (Enumerable.Range(lowerBand, upperBand).Contains(argJobs.Count)){
                    numberOfPages = i + 1;
                    break;
                }
            }
            
            for (int i = firstNumberInArray; i < numberOfPages + 1; i++){
                numbersToReturn.Add(i);
            }

            return numbersToReturn;
        }


        public List<Job> GetJobsForPage(int pageNumber, List<Job> argJobs){
            List<Job> jobsToreturn = new List<Job>();

            int startingIndex = (pageNumber -1) * JobCountPerPage;

            if (argJobs.Count < JobCountPerPage){
                for (int i = startingIndex; i < argJobs.Count; i++){
                    jobsToreturn.Add(argJobs[i]);
                }
                return jobsToreturn;
            }

            if (startingIndex < JobCountPerPage){
                for (int i = startingIndex; i < JobCountPerPage; i++){
                    jobsToreturn.Add(argJobs[i]);
                }
                return jobsToreturn;
            }

            for (int i = startingIndex; i < startingIndex + (argJobs.Count - startingIndex); i++){
                jobsToreturn.Add(argJobs[i]);
            }

            return jobsToreturn;
        }

        public string JobClassName(string jobType){
            if (jobType == "Help Needed"){
                return "card text-white bg-secondary mb-3";
            }
            return "card text-white bg-primary mb-3";
        }

        public string JobColour(string jobType){
            if (jobType == "Help Needed"){
                return "orange";
            }
            return "blue";
        }
    }
}