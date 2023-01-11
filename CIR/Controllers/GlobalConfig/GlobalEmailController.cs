using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalEmailController : ControllerBase
    {
        private readonly IGlobalEmailService _emailService;
        public GlobalEmailController(IGlobalEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<GlobalConfigurationEmailsModel> globalEmailModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addGlobalEmail = _emailService.SaveGlobalEmail(globalEmailModel);
                    if (addGlobalEmail == "Success")
                    {
                        return Ok(new { message = addGlobalEmail });
                    }
                    else
                    {
                        return BadRequest(new { message = "Error occured Invalid id provided." });
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }

            }
            return BadRequest();
        }
        [HttpGet("{id}")]
        public async Task<CustomResponse<GlobalConfigurationEmailsGetModel>> Get(int id)
        {
            try
            {
                var user = await _emailService.globalEmailGetData(id);
                if (user != null)
                {
                    return new CustomResponse<GlobalConfigurationEmailsGetModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = user };
                }
                return new CustomResponse<GlobalConfigurationEmailsGetModel>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = user };

            }
            catch
            {
                return new CustomResponse<GlobalConfigurationEmailsGetModel>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError };
            }
        }
    }
}
