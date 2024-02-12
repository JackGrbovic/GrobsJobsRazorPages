using GrobsJobsRazorPages.CustomServices;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrobsJobsRazorPages.Pages.UserPages
{
    public class InboxModel : PageModel
    {
        public MessageHandler MessageHandler { get; set; }

        public UserHandler UserHandler { get; set; }

        public IdentityUser LoggedInUser { get; set; }

        public List<IdentityUser> MessagedUsers { get; set; }

        public List<Message> Inbox { get; set; }

        public List<Message> PreviewMessages { get; set; }

        public List<string> NormalizedUserNamesWithoutLoggedInUser = new List<string>();

        public List<string> IdsWithoutLoggedInUser = new List<string>();

        public List<string> NormalizedUserNamesWithLoggedInUser = new List<string>();

        public Dictionary<string, Message> PreviewMessagesAndNames = new Dictionary<string, Message>();

        public InboxModel(MessageHandler messageHandler, UserHandler userHandler)
        {
            MessageHandler = messageHandler;
            UserHandler = userHandler;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated){
                LoggedInUser = UserHandler.ReturnFirstPartyUser(User);
                Inbox = MessageHandler.GetUserInbox(LoggedInUser);
                MessagedUsers = MessageHandler.GetUsersFromMessagesInInbox(Inbox, LoggedInUser);
                PreviewMessages = MessageHandler.GetPreviewMessages(MessagedUsers, LoggedInUser);

                NormalizedUserNamesWithoutLoggedInUser = MessageHandler.GetNormalizedUserNameSansLoggedInUserSenderOrRecipient(PreviewMessages, LoggedInUser);
                NormalizedUserNamesWithLoggedInUser = MessageHandler.GetNormalizedUserNameWithLoggedInUserOnlySender(PreviewMessages);
                IdsWithoutLoggedInUser = MessageHandler.GetIdsSansLoggedInUserSenderOrRecipient(PreviewMessages, LoggedInUser);

                return Page();
            }
            return RedirectToPage("/Error", new { errorMessage = "You must be signed in to access your inbox." });
        }
    }
}
