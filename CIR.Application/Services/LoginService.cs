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
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			return await _loginRepository.ForgotPassword(forgotPasswordModel);
		}
		public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			return await _loginRepository.ResetPassword(resetPasswordModel);
		}
	}
}
