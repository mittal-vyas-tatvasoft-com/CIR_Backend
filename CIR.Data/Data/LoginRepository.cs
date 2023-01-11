using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Common.MailTemplate;
using CIR.Common.SystemConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomStringCreator;

namespace CIR.Data.Data
{
	public class LoginRepository : ILoginRepository
	{
		private readonly CIRDbContext _CIRDBContext;
		private readonly EmailGeneration _emailGeneration;
		private readonly JwtGenerateToken _jwtGenerateToken;
		public LoginRepository(CIRDbContext context, EmailGeneration emailGeneration, JwtGenerateToken jwtGenerateToken)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
			_emailGeneration = emailGeneration;
			_jwtGenerateToken = jwtGenerateToken;
		}
		/// <summary>
		/// This method used by login user
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<IActionResult> Login(LoginModel model)
		{
			try
			{
				var fetchedrecord = _CIRDBContext.Users.Where((x) => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();

				if (fetchedrecord != null && fetchedrecord.ResetRequired == true)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Forbidden, Result = false, Message = HttpStatusCodesMessages.Forbidden, Data = "Your Account is locked. To Unlock this account contact to Aramex Support team" });
				}
				User user = new User()
				{
					UserName = model.UserName,
				};
				if (fetchedrecord == null)
				{
					var userdetails = _CIRDBContext.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

					var temp = (from item in _CIRDBContext.Users
								where item.UserName == model.UserName
								select item.Id);

					if (userdetails.LoginAttempts < 5)
					{
						userdetails.Id = temp.FirstOrDefault();
						userdetails.LoginAttempts += 1;
						_CIRDBContext.Entry(userdetails).State = EntityState.Modified;
						_CIRDBContext.SaveChanges();
					}
					else
					{
						userdetails.ResetRequired = true;
						_CIRDBContext.Users.Update(userdetails);
						_CIRDBContext.SaveChanges();
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Forbidden, Result = false, Message = HttpStatusCodesMessages.Forbidden, Data = "Your Account is locked. To Unlock this account contact to Aramex Support team" });
					}
				}
				var userdata = _CIRDBContext.Users.FirstOrDefault((u) => u.UserName == model.UserName && u.Password == model.Password);
				if (userdata != null)
				{
					var generatedToken = await _jwtGenerateToken.GenerateJwtToken(userdata);
					if (generatedToken != null)
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = generatedToken });
					else
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Token not generated" });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Invalid username or password" });
			}
			catch (Exception ex)
			{

				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method used by forgot password
		/// </summary>
		/// <param name="forgotPasswordModel"></param>
		/// <returns>Success status if its valid else failure</returns>
		public string ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			var user = _CIRDBContext.Users.Where(c => c.UserName == forgotPasswordModel.UserName && c.Email == forgotPasswordModel.Email).FirstOrDefault();
			if (user != null)
			{
				string randomString = SystemConfig.randomString;
				string newPassword = new StringCreator(randomString).Get(8);

				_CIRDBContext.Users.Where(x => x.Id == user.Id).ToList().ForEach((a =>
				{
					a.Password = newPassword;
				}
				));
				_CIRDBContext.SaveChanges();

				//Send NewPassword in Mail
				string mailSubject = MailTemplate.ForgotPasswordSubject();
				string mailBody = MailTemplate.ForgotPasswordTemplate(user);
				_emailGeneration.SendMail(forgotPasswordModel.Email, mailSubject, mailBody);

				return "Success";
			}
			return "Failure";
		}

		/// <summary>
		/// This method used by reset password
		/// </summary>
		/// <param name="resetPasswordModel"></param>
		/// <returns>Success status if its valid else failure</returns>
		public string ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			var user = _CIRDBContext.Users.Where(c => c.Id == resetPasswordModel.Id).FirstOrDefault();
			if (user != null)
			{
				if (user.Password == resetPasswordModel.OldPassword)
				{
					user.Id = resetPasswordModel.Id;
					user.Password = resetPasswordModel.NewPassword;
					_CIRDBContext.Users.Update(user);
					_CIRDBContext.SaveChanges();
					return "Success";
				}
			}
			return "Failure";
		}

	}
}
