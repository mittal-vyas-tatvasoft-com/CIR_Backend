using System.ComponentModel.DataAnnotations;

namespace CIR.Core.ViewModel
{
    public class ResetPasswordModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
