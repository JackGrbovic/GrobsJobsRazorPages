using FluentValidation;
using GrobsJobsRazorPages.Model;

namespace GrobsJobsRazorPages.CustomServices
{
    public class JobValidator : AbstractValidator<Job>
    {
        public JobValidator()
        {
            RuleFor(j => j.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(j => j.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(j => j.JobType).NotEmpty().WithMessage("JobType is required.");
            RuleFor(j => j.JobPosterNormalizedUserName).NotEmpty().WithMessage("JobPosterNormalizedUserName is required.");
            RuleFor(j => j.JobPosterId).NotEmpty().WithMessage("JobPosterId is required.");
            RuleFor(j => j.DateTimePosted).NotEmpty().WithMessage("DateTimePosted is required.");
        }
    }
}
