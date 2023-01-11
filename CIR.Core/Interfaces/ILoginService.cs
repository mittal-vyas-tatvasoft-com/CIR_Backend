using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces
{
	public interface ILoginService
	{
		Task<IActionResult> Login(LoginModel model);
		public string ForgotPassword(ForgotPasswordModel forgotPasswordModel);
		public string ResetPassword(ResetPasswordModel resetPasswordModel);
	}
}
