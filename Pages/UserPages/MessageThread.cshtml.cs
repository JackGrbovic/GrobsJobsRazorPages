using FluentValidation;
using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class MessageThreadModel : PageModel
    {
        public IdentityUser FirstParty { get; set; }
        public IdentityUser SecondParty { get; set; }

        public List<Message> FirstPartyMessages { get; set; }
        public List<Message> SharedMessages { get; set; }

        [BindProperty]
        public Message MessageToSend { get; set; }

        private GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        private IValidator<Message> MessageValidator { get; set; }

        public UserHandler GetUser { get; set; }

        public MessageHandler MessageHandler { get; set; }

        public MessageThreadModel(MessageHandler messageHandler, UserHandler getUser, GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext, IValidator<Message> messageValidator) 
        {
            MessageHandler = messageHandler;
            GetUser = getUser;
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
            MessageValidator = messageValidator;
        }

        public void OnGet(string secondPartyId)
        {
            FirstParty = GetUser.ReturnFirstPartyUser(User);
            SecondParty = GetUser.ReturnSecondPartyUserById(secondPartyId);
            FirstPartyMessages = MessageHandler.GetUserInbox(FirstParty);
            SharedMessages = MessageHandler.GetMessageThreadOfTwoUsers(secondPartyId, FirstPartyMessages);
        }

        public async Task<IActionResult> OnPost(string secondPartyId)
        {
            //put together problem details as necessary
            FirstParty = GetUser.ReturnFirstPartyUser(User);
            SecondParty = GetUser.ReturnSecondPartyUserById(secondPartyId);
            FirstPartyMessages = MessageHandler.GetUserInbox(FirstParty);
            SharedMessages = MessageHandler.GetMessageThreadOfTwoUsers(secondPartyId, FirstPartyMessages);

            MessageToSend.MessageTitle = SharedMessages[0].MessageTitle;
            MessageToSend.MessageRecipient = SecondParty.Id;
            MessageToSend.MessageSender = FirstParty.Id;
            MessageToSend.MessageRecipientUserName = SecondParty.NormalizedUserName;
            MessageToSend.MessageSenderUserName = FirstParty.NormalizedUserName;
            MessageToSend.DateTimeSent = DateTime.Now;

            var messageValidation = MessageValidator.Validate(MessageToSend);

            if (messageValidation.IsValid)
            {
                await MessageHandler.SendMessage(GrobsJobsRazorPagesDbContext, MessageToSend);
                return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
            }
            //this is where we can put an error message
            return RedirectToPage("/UserPages/MessageThread", new { secondPartyId });
        }
    }
}
