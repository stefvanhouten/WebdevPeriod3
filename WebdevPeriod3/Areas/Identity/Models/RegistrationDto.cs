using System.ComponentModel.DataAnnotations;

namespace WebdevPeriod3.Areas.Identity.Models
{
    public class RegistrationDto
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Compare(nameof(Password), ErrorMessage = "The passwords should match.")]
        [Display(Name = "Password (repeat)")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}
