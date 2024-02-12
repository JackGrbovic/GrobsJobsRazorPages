using FluentValidation;
using GrobsJobsRazorPages.Model;

namespace GrobsJobsRazorPages.CustomServices
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator() {

            RuleFor(m => m.MessageSender).NotEmpty().WithMessage("Sender is required.");
            RuleFor(m => m.MessageRecipient).NotEmpty().WithMessage("Recipient is required.");
            RuleFor(m => m.MessageSenderUserName).NotEmpty().WithMessage("Sender is required.");
            RuleFor(m => m.MessageRecipientUserName).NotEmpty().WithMessage("Recipient is required.");
            RuleFor(m => m.MessageTitle).NotEmpty().WithMessage("Title is required.");
            RuleFor(m => m.MessageBody).NotEmpty().WithMessage("Body is required.");
            RuleFor(m => m.DateTimeSent).NotEmpty().WithMessage("DateTime is required.");
        }
    }
}
