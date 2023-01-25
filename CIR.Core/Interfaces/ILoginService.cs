using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces
{
    public interface ILoginService
    {
        Task<IActionResult> Login(LoginModel model);
        Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel);
        Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel);
    }
}
