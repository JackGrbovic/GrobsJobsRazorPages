using FluentValidation;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class StartMessageThreadModel : PageModel
    {
        //the value of _firstParty is being correctly attributed and then is later being set to null for some reason
        public IdentityUser FirstParty { get ; set; }
        public IdentityUser SecondParty { get; set; }

        [BindProperty]
        public Message MessageToSend { get; set; }

        public string JobName = "";

        public string RecipientName = "";

        public UserHandler GetUser { get; set; }

        public List<Message> Inbox { get; set; }

        private IValidator<Message> MessageValidator { get; set; }

        private GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        public MessageHandler MessageHandler { get; set; }

        public StartMessageThreadModel(MessageHandler messageHandler, UserHandler getUser, GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, IValidator<Message> messageValidator)
        {
            MessageHandler = messageHandler;
            GetUser = getUser;
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            MessageValidator = messageValidator;
        }

        //this has been set to empty because there could be a message thread started without a preselected recipient (the recipient is added retrospectively)
        public void OnGet(string recipientNormalizedUserName = "Empty", string jobName = "Empty")
        {
            FirstParty = GetUser.ReturnFirstPartyUser(User);
            SecondParty = GetUser.ReturnSecondPartyUserByNormalizedUserName(recipientNormalizedUserName);

            if (recipientNormalizedUserName != "Empty")
            {
                RecipientName = recipientNormalizedUserName;
            }
            if (jobName != "Empty")
            {
                JobName = jobName;
            }
        }

        public async Task<IActionResult> OnPost(string secondPartyId)
        {
            FirstParty = GetUser.ReturnFirstPartyUser(User);
            SecondParty = GetUser.ReturnSecondPartyUserById(secondPartyId);

            var problemDetails = new ProblemDetails
            {
                Status = 404,
                Title = "An error occurred",
                Detail = "Recipient's email is either incorrect or does not exist. Reload the page and try again.",
                Instance = HttpContext.Request.Path
            };

            if (SecondParty.Id == null)
            {
                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            }

            MessageToSend.MessageRecipient = SecondParty.Id;
            MessageToSend.MessageSender = FirstParty.Id;
            MessageToSend.MessageRecipientUserName = SecondParty.NormalizedUserName;
            MessageToSend.MessageSenderUserName = FirstParty.NormalizedUserName;
            MessageToSend.DateTimeSent = DateTime.Now;
            
            var validationResult = await MessageValidator.ValidateAsync(MessageToSend);

            if (validationResult.IsValid)
            {
                MessageHandler.SendMessage(GrobsJobsRazorPagesDbContext, MessageToSend);
                //instead of returning the same page we need to return the message thread
                return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
            }
            //this return statement needs to take us to a page that just says "Message Sent"
            //the error message also needs to be whatever the validation issue is in the validator
            string errorMessage = "There was a problem submitting your message. Please try again.";
            return RedirectToPage("/Error", new { errorMessage });
        }
    }
}
