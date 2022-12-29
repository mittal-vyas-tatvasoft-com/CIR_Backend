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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel value)
        {
            if (ModelState.IsValid)
            {
                var user = _loginService.Login(value);
                if (user != null)
                {
                    var generatedToken = await GenerateJwtToken(user);
                    if (generatedToken != null)
                        return Ok(new { token = generatedToken });
                    else
                        return BadRequest(new { message = "Token not generated" });
                }
                else
                {
                    return NotFound(new { message = "Username or password is incorrect" });
                }
            }
            return BadRequest();
        }

        private async Task<string> GenerateJwtToken(CIR.Core.Entities.User user)
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
       
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var forgotPassword = _loginService.ForgotPassword(forgotPasswordModel);
                if(forgotPassword != "")
                {
                    return Ok("Your New Password is : " + forgotPassword);
                }
                return NotFound("Not Valid UserName and Email");
            }
            return BadRequest();
        }      

    }
}
