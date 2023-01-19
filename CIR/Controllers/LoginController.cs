using CIR.Common.CustomResponse;
using CIR.Core.Interfaces;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
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
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
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
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }
    }
}
