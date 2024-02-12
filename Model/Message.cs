using System.ComponentModel.DataAnnotations;

namespace GrobsJobsRazorPages.Model
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MessageTitle { get; set; }
        [Required]
        public string MessageBody { get; set; }
        [Required]
        public string MessageSender { get; set; }
        [Required]
        public string MessageRecipient { get; set; }
        [Required]
        public string MessageSenderUserName { get; set; }
        [Required]
        public string MessageRecipientUserName { get; set; }
        [Required]
        public DateTime DateTimeSent { get; set; }
    }
}
