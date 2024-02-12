using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GrobsJobsRazorPages.CustomServices
{
    public class UserHandler
    {
        public SignInManager<IdentityUser> SignInManager { get; set; }

        public AuthDbContext AuthDbContext { get; set; }

        public UserHandler(SignInManager<IdentityUser> signInManager, AuthDbContext authDbContext)
        {
            SignInManager = signInManager;
            AuthDbContext = authDbContext;
        }

        public bool IsUserLoggedIn(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IdentityUser? ReturnFirstPartyUser(ClaimsPrincipal user)
        {
            ClaimsPrincipal currentUser = user;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserID == null) return null;
            var aspUser = AuthDbContext.Users
                .Single(b => b.Id == currentUserID);
            return aspUser;
        }

        public IdentityUser ReturnSecondPartyUserById(string secondPartyId)
        {
            IdentityUser userToReturn = new IdentityUser();

            var userById = AuthDbContext.Users
                .Single(b => b.Id == secondPartyId);

            if (userById != null)
            {
                userToReturn = userById;
            }
            return userToReturn;
        }

        public IdentityUser ReturnSecondPartyUserByNormalizedUserName(string secondPartyNormalizedUserName)
        {
            secondPartyNormalizedUserName = secondPartyNormalizedUserName.ToUpper();
            IdentityUser userToReturn = new IdentityUser();
       
            var userByNormalizedUserName = AuthDbContext.Users
                .Single(b => b.NormalizedUserName == secondPartyNormalizedUserName);

            if (userByNormalizedUserName != null)
            {
                userToReturn = userByNormalizedUserName;
            }
            return userToReturn;
        }

        public bool ValidateClaimsPriciple(ClaimsPrincipal user)
        {
            if (user.Identity.Name != null)
            {
                return true;
            }
            return false;
        }

        public bool IsRegistrationStringValid(string stringToValidate)
        {
            List<string> normalizedUserNames = AuthDbContext.Users.Select(u => u.NormalizedUserName).ToList();
            List<string> emailAddresses = AuthDbContext.Users.Select(u => u.Email).ToList();
            if (normalizedUserNames.Contains(stringToValidate.ToUpper()))
            {
                return false;
            }
            else if (emailAddresses.Contains(stringToValidate))
            {
                return false;
            }
            return true;
        }
    }
}
