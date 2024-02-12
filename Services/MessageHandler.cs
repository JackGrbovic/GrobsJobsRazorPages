using GrobsJobsRazorPages.Data;
using GrobsJobsRazorPages.Model;
using Microsoft.AspNetCore.Identity;

namespace GrobsJobsRazorPages.CustomServices
{
    public class MessageHandler
    {
        private AuthDbContext AuthDbContext { get; set; }
        private GrobsJobsRazorPagesDbContext GrobsJobsRazorPagesDbContext { get; set; }

        public MessageHandler(AuthDbContext authDbContext, GrobsJobsRazorPagesDbContext grobsJobsRazorPagesDbContext)
        {
            AuthDbContext = authDbContext;
            GrobsJobsRazorPagesDbContext = grobsJobsRazorPagesDbContext;
        }

        public Message CreateMessage(string messageSender, string messageRecipient, string messageTitle, string messageBody)
        {
            Message message = new Message{
                MessageSender = messageSender,
                MessageRecipient = messageRecipient,
                MessageTitle = messageTitle,
                MessageBody = messageBody,
                DateTimeSent = DateTime.Now
            };
            
            return message;
        }

        public async Task SendMessage(GrobsJobsRazorPagesDbContext context, Message messageToSend)
        {
            await context.Messages.AddAsync(messageToSend);
            await context.SaveChangesAsync();
        }

        public List<Message> GetUserInbox(IdentityUser user)
        {
            List<Message> messages = new List<Message>();

            var receivedMessages = GrobsJobsRazorPagesDbContext.Messages.Where(x => x.MessageRecipient == user.Id).ToList();
            var sentMessages = GrobsJobsRazorPagesDbContext.Messages.Where(x => x.MessageSender == user.Id).ToList();

            messages.AddRange(receivedMessages);
            messages.AddRange(sentMessages);
            return messages;
        }

        public int GetUserInboxCount(IdentityUser recipient)
        {
            var messageCollection = GrobsJobsRazorPagesDbContext.Messages.Where(x => x.MessageRecipient == recipient.UserName).ToList();
            int messageCount = messageCollection.Count();
            return messageCount;
        }

        public List<Message> GetUserOutbox(IdentityUser sender)
        {
            var messageCollection = GrobsJobsRazorPagesDbContext.Messages.Where(x => x.MessageSender == sender.UserName).ToList();
            return messageCollection;
        }

        public List<Message> GetMessageThreadOfTwoUsers(string secondPartyId, List<Message> firstPartyMessages)
        {
            List<Message> listToReturn = new List<Message>();

            var messagesByRecipient = firstPartyMessages.Where(m => m.MessageRecipient == secondPartyId).ToList();
            var messagesBySender = firstPartyMessages.Where(m => m.MessageSender == secondPartyId).ToList();

            //this method needs to return the messages of the other user as well

            listToReturn.AddRange(messagesByRecipient);
            listToReturn.AddRange(messagesBySender);
            listToReturn.OrderByDescending(m => m.DateTimeSent).ToList();

            return listToReturn;
        }

        public List<string> GetNormalizedUserNameSansLoggedInUserSenderOrRecipient(List<Message> previewMessages, IdentityUser loggedInUser)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                if (message.MessageSender != loggedInUser.Id)
                {
                    string normalizedUserName = AuthDbContext.Users.Where(u => u.Id == message.MessageSender).FirstOrDefault().NormalizedUserName;
                    valuesToReturn.Add(normalizedUserName);
                }
                else
                {
                    string normalizedUserName = AuthDbContext.Users.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().NormalizedUserName;
                    valuesToReturn.Add(normalizedUserName);
                }
            }
            return valuesToReturn;
        }

        public List<string> GetIdsSansLoggedInUserSenderOrRecipient(List<Message> previewMessages, IdentityUser loggedInUser)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                if (message.MessageSender != loggedInUser.Id)
                {
                    string normalizedUserName = AuthDbContext.Users.Where(u => u.Id == message.MessageSender).FirstOrDefault().Id;
                    valuesToReturn.Add(normalizedUserName);
                }
                else
                {
                    string normalizedUserName = AuthDbContext.Users.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().Id;
                    valuesToReturn.Add(normalizedUserName);
                }
            }
           return valuesToReturn;
        }

        public List<string> GetNormalizedUserNameWithLoggedInUserOnlySender(List<Message> previewMessages)
        {
            List<string> valuesToReturn = new List<string>();
            foreach (Message message in previewMessages)
            {
                string normalizedUserName = AuthDbContext.Users.Where(u => u.Id == message.MessageRecipient).FirstOrDefault().NormalizedUserName;
                valuesToReturn.Add(normalizedUserName);
            }
           return valuesToReturn;
        }

        public List<IdentityUser> GetUsersFromMessagesInInbox(List<Message> inbox, IdentityUser user)
        {
            List<IdentityUser> userList = new List<IdentityUser>();
            IdentityUser userToAdd = new IdentityUser();

            //we want to return every user that has been involved on one user's inbox, including the loggedinuser

            foreach (Message message in inbox)
            {
                var messageSender = AuthDbContext.Users.Where(u => u.Id == message.MessageSender).FirstOrDefault();
                var messageRecipient = AuthDbContext.Users.Where(u => u.Id == message.MessageRecipient).FirstOrDefault();

                bool messageSenderAlreadyInList = false;
                bool messageRecipientAlreadyInList = false;

                if (userList.Count == 0)
                {
                    userList.Add(messageSender);
                    userList.Add(messageRecipient);
                }

                else
                {
                    if (userList.Contains(messageSender))
                    {
                        messageSenderAlreadyInList = true;
                    }
                    if (userList.Contains(messageRecipient))
                    {
                        messageRecipientAlreadyInList = true;
                    }

                    if (messageSenderAlreadyInList == false && messageSender.Id != null)
                    {
                        userList.Add(messageSender);
                    }
                    if (messageRecipientAlreadyInList == false && messageRecipient.Id != null)
                    {
                        userList.Add(messageRecipient);
                    }
                }
            }
            return userList;
        }

        public Message ShortenMessage(Message messageToShorten)
        {
            Message messageToReturn;
            messageToReturn = messageToShorten;
            string shortenedMessage = "";
            if (messageToShorten.MessageBody.Length > 100)
            {
                for (int i = 0; i < 100; i++)
                {
                    shortenedMessage += messageToShorten.MessageBody[i];
                }
                shortenedMessage += "...";
                messageToReturn.MessageBody = shortenedMessage;
            }
            return messageToReturn;
        }

        public List<Message> GetPreviewMessages(List<IdentityUser> messagedUsers, IdentityUser loggedInUser)
        {
            //List<Message> latestMessages = new List<Message>();
            //List<string> recipients = JobPostDbContext.Message.Where(x => x.MessageSender == loggedInUser.Id).Select(x => x.MessageRecipient).Distinct().ToList();

            //foreach(string recipient in recipients)
            //{
            //    latestMessages.Add(JobPostDbContext.Message.Where(x => x.MessageRecipient == recipient).OrderByDescending(x => x.DateTimeSent).First());
            //}

            //return latestMessages;


            List<Message> previewMessages = new List<Message>();

            for (int i = 0; i < messagedUsers.Count; i++)
            {
                var mostRecentMessageFromLoggedInUser = GrobsJobsRazorPagesDbContext.Messages
                                    .Where(x => x.MessageSender == loggedInUser.Id)
                                    .OrderByDescending(x => x.DateTimeSent)
                                    .FirstOrDefault();

                var mostRecentMessageFromOtherUser = GrobsJobsRazorPagesDbContext.Messages
                                    .Where(x => x.MessageSender == messagedUsers[i].Id)
                                    .OrderByDescending(x => x.DateTimeSent)
                                    .FirstOrDefault();

                if (mostRecentMessageFromLoggedInUser != null && mostRecentMessageFromOtherUser != null)
                {
                    if (mostRecentMessageFromLoggedInUser.DateTimeSent > mostRecentMessageFromOtherUser.DateTimeSent)
                    {
                        ShortenMessage(mostRecentMessageFromLoggedInUser);
                        previewMessages.Add(mostRecentMessageFromLoggedInUser);
                    }
                    else if (mostRecentMessageFromLoggedInUser.DateTimeSent < mostRecentMessageFromOtherUser.DateTimeSent)
                    {
                        ShortenMessage(mostRecentMessageFromOtherUser);
                        previewMessages.Add(mostRecentMessageFromOtherUser);
                    }
                }

                else if (mostRecentMessageFromLoggedInUser != null && mostRecentMessageFromOtherUser == null)
                {
                    ShortenMessage(mostRecentMessageFromLoggedInUser);
                    previewMessages.Add(mostRecentMessageFromLoggedInUser);
                }

                else if (mostRecentMessageFromOtherUser != null && mostRecentMessageFromLoggedInUser == null)
                {
                    ShortenMessage(mostRecentMessageFromOtherUser);
                    previewMessages.Add(mostRecentMessageFromOtherUser);
                }
            }
            return previewMessages;
        }
    }
}
