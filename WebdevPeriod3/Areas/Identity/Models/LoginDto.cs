using System.ComponentModel.DataAnnotations;

namespace WebdevPeriod3.Areas.Identity.Models
{
    public class LoginDto
    {
        public LoginDto() { }

        public LoginDto(string returnUrl) : this()
        {
            ReturnUrl = returnUrl;
        }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Stay signed in")]
        public bool RemainSignedIn { get; set; }
        public string ReturnUrl { get; set; }
    }
}

