using System.ComponentModel.DataAnnotations;

namespace CIR.Core.ViewModel
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
