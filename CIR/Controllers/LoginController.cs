using CIR.Application.Interfaces;
using CIR.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel value)
        {
            if (ModelState.IsValid)
            {
                var user = _loginService.Login(value);
                if (user != null)
                {
                    return Ok(ModelState);
                } else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }
    }
}
