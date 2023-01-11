using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services
{
	public class LoginService : ILoginService
	{
		private readonly ILoginRepository _loginRepository;
		public LoginService(ILoginRepository loginRepository)
		{
			_loginRepository = loginRepository;
		}
		public async Task<IActionResult> Login(LoginModel model)
		{
			return await _loginRepository.Login(model);
		}
		public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			return _loginRepository.ForgotPassword(forgotPasswordModel);
		}
		public string ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			return _loginRepository.ResetPassword(resetPasswordModel);
		}
	}
}
