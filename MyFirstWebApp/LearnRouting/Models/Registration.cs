using System.ComponentModel.DataAnnotations;

namespace LearnRouting.Models
{
    public class Registration
    {
        [Required]
        [EmailAddress(ErrorMessage = "A valid email is required.")]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }

    }
}
