using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomStringCreator;
using System.Data;

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
				var userRecords = _CIRDBContext.Users.Where((x) => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();

				if (userRecords != null && userRecords.ResetRequired == true)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Forbidden, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.Forbidden.GetDescriptionAttribute(), Data = SystemMessages.msgAccountIsLocked });
				}
				if (userRecords == null)
				{
					var userDetails = _CIRDBContext.Users.Where(x => x.Email == model.Email).FirstOrDefault();
					if (userDetails != null)
					{
						var userId = (from item in _CIRDBContext.Users where item.Email == model.Email select item.Id);
						if (userDetails.LoginAttempts < 5)
						{
							var result = 0;
							using (DbConnection dbConnection = new DbConnection())
							{
								using (var connection = dbConnection.Connection)
								{
									DynamicParameters parameters = new DynamicParameters();
									parameters.Add("@userId", userId.FirstOrDefault());
									result = connection.Execute("spIncrementLoginAttempts", parameters, commandType: CommandType.StoredProcedure);
								}
							}
							if (result != 0)
							{
								return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgInvalidUserNameOrPassword) });
							}
						}
						else
						{
							var result = 0;
							using (DbConnection dbConnection = new DbConnection())
							{
								using (var connection = dbConnection.Connection)
								{
									DynamicParameters parameters = new DynamicParameters();
									parameters.Add("@userId", userId.FirstOrDefault());
									result = connection.Execute("spResetRequired", parameters, commandType: CommandType.StoredProcedure);
								}
							}
							if (result >= 1)
							{
								return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Forbidden, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.Forbidden.GetDescriptionAttribute(), Data = SystemMessages.msgAccountIsLocked });
							}
						}
					}
					else
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidUserNameOrPassword });
					}
				}
				var userData = _CIRDBContext.Users.FirstOrDefault((u) => u.Email == model.Email && u.Password == model.Password);
				if (userData != null)
				{
					var generatedToken = await _jwtGenerateToken.GenerateJwtToken(userData);
					if (generatedToken != null)
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = generatedToken });
					else
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgTokenNotGenerated });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidUserNameOrPassword });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method used by forgot password
		/// </summary>
		/// <param name="forgotPasswordModel"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		{
			try
			{
				var user = _CIRDBContext.Users.Where(c => c.UserName == forgotPasswordModel.UserName && c.Email == forgotPasswordModel.Email).FirstOrDefault();
				if (user != null)
				{
					string randomString = SystemConfig.randomString;
					string newPassword = new StringCreator(randomString).Get(8);

					_CIRDBContext.Users.Where(x => x.Id == user.Id).ToList().ForEach(a =>
					{
						a.Password = newPassword;
						a.ResetRequired = true;
					}
					);
					await _CIRDBContext.SaveChangesAsync();

					//Send NewPassword in Mail
					string mailSubject = EmailGeneration.ForgotPasswordSubject();
					string mailBody = EmailGeneration.ForgotPasswordTemplate(user);
					_emailGeneration.SendMail(forgotPasswordModel.Email, mailSubject, mailBody);

					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = SystemMessages.msgSendNewPasswordOnMail });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgEnterValidUserNameAndEmail });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method used by reset password
		/// </summary>
		/// <param name="resetPasswordModel"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
		{
			try
			{
				var user = _CIRDBContext.Users.Where(c => c.Email == resetPasswordModel.Email).FirstOrDefault();

				if (user != null)
				{
					if (user.Password == resetPasswordModel.OldPassword)
					{
						var result = 0;
						using (DbConnection dbConnection = new DbConnection())
						{
							using (var connection = dbConnection.Connection)
							{
								DynamicParameters parameters = new DynamicParameters();
								parameters.Add("@Email", resetPasswordModel.Email);
								parameters.Add("@Password", resetPasswordModel.NewPassword);
								parameters.Add("@ResetRequired", false);
								result = connection.Execute("spResetPassword", parameters, commandType: CommandType.StoredProcedure);
							}
						}
						if (result != 0)
						{
							return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Reset Succesfully..") });
						}
					}
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgIncorrectOldPassword });
				}
				else
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidEmailAddress });
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}


	}
}
