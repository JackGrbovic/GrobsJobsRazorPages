using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;

namespace GrobsJobsRazorPages.Model
{
    public class Inbox
    {
        public IdentityUser _identityUser { get; set; }

        public List<Message> _messages { get; set; }

        public Inbox(IdentityUser identityUser) 
        {
            this._identityUser = identityUser;
        }  
    }
}
