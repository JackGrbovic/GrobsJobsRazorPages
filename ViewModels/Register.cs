using System.ComponentModel.DataAnnotations;

namespace GrobsJobsRazorPages.ViewModel
{
    public class Register
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
