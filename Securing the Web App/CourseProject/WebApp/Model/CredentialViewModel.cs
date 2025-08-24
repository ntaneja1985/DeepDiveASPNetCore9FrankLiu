using System.ComponentModel.DataAnnotations;

namespace WebApp.Model
{
    public class CredentialViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
