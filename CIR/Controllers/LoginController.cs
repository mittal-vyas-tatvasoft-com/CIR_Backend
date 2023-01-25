using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CIR.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly ILoginService _loginService;
		private readonly AppSettings _appSettings;

		public LoginController(ILoginService loginService, IOptions<AppSettings> settings)
		{
			_loginService = loginService;
			_appSettings = settings.Value;
		}

		/// <summary>
		/// This method takes login user
		/// </summary>
		/// <param name="value">this object contains different parameters as details of a login user</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginModel value)
		{
			if (ModelState.IsValid)
			{
				return await _loginService.Login(value);
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes generate Jwt token
		/// </summary>
		/// <param name="user">this object contains different parameters as details of a user</param>
		/// <returns></returns>
		private async Task<string> GenerateJwtToken(User user)
		{
			string jwtToken = string.Empty;
			try
			{
				// generate token that is valid for 20 minutes
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_appSettings.AuthKey);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new[]
						{
						new Claim("Id", user.Id.ToString()),
						new Claim("UserName", user.UserName),
						new Claim("FirstName", user.FirstName),
						new Claim("LastName", user.LastName),
					}),
					Expires = DateTime.UtcNow.AddMinutes(20),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				jwtToken = tokenHandler.WriteToken(token);
			}
			catch (Exception)
			{
				throw;
			}
			return await Task.Run(() =>
			{
				return jwtToken;
			});
		}

		/// <summary>
		/// This method takes forgot password
		/// </summary>
		/// <param name="forgotPasswordModel">this object contains different parameters as details of a forgot password</param>
		/// <returns></returns>
		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _loginService.ForgotPassword(forgotPasswordModel);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes reset password
		/// </summary>
		/// <param name="resetPasswordModel">this object contains different parameters as details of a reset password</param>
		/// <returns></returns>
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _loginService.ResetPassword(resetPasswordModel);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}
	}
}
