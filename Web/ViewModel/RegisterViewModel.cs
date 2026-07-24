using System.ComponentModel.DataAnnotations;

namespace Web.ViewModel
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; } = "Reader";
    }
}
