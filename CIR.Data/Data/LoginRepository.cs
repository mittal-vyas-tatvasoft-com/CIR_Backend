using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Common.MailTemplate;
using CIR.Common.SystemConfig;
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
                var userRecords = _CIRDBContext.Users.Where((x) => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();

                if (userRecords != null && userRecords.ResetRequired == true)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Forbidden, Result = false, Message = HttpStatusCodesMessages.Forbidden, Data = "Your Account is locked. To Unlock this account contact to Aramex Support team" });
                }
                if (userRecords == null)
                {
                    var userDetails = _CIRDBContext.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                    if (userDetails != null)
                    {
                        var userId = from item in _CIRDBContext.Users where item.Email == model.Email select item.Id;
                        if (userDetails.LoginAttempts < 5)
                        {
                            userDetails.Id = userId.FirstOrDefault();
                            userDetails.LoginAttempts += 1;
                            _CIRDBContext.Entry(userDetails).State = EntityState.Modified;
                            _CIRDBContext.SaveChanges();
                        }
                        else
                        {
                            userDetails.ResetRequired = true;
                            _CIRDBContext.Users.Update(userDetails);
                            _CIRDBContext.SaveChanges();
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Forbidden, Result = false, Message = HttpStatusCodesMessages.Forbidden, Data = "Your Account is locked. To Unlock this account contact to Aramex Support team" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Invalid username or password" });
                    }
                }
                var userData = _CIRDBContext.Users.FirstOrDefault((u) => u.Email == model.Email && u.Password == model.Password);
                if (userData != null)
                {
                    var generatedToken = await _jwtGenerateToken.GenerateJwtToken(userData);
                    if (generatedToken != null)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = generatedToken });
                    }
                    else
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Token not generated" });
                    }
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
                    string mailSubject = MailTemplate.ForgotPasswordSubject();
                    string mailBody = MailTemplate.ForgotPasswordTemplate(user);
                    _emailGeneration.SendMail(forgotPasswordModel.Email, mailSubject, mailBody);

                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Successfully send new password on your mail,please check once!" });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Please enter valid username and email" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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
                        user.Email = resetPasswordModel.Email;
                        user.Password = resetPasswordModel.NewPassword;
                        user.ResetRequired = false;
                        _CIRDBContext.Users.Update(user);
                        await _CIRDBContext.SaveChangesAsync();
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Password Change Successfully." });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "OldPassword InCorrect." });
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid Email Address." });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }


    }
}
