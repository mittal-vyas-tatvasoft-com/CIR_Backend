using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalConfigurationEmailsController : ControllerBase
    {
        #region PROPERTIES
        private readonly IGlobalConfigurationEmailsService _globalConfigurationEmailsService;
        #endregion

        #region CONSTRUCTORS
        public GlobalConfigurationEmailsController(IGlobalConfigurationEmailsService globalConfigurationEmailsService)
        {
            _globalConfigurationEmailsService = globalConfigurationEmailsService;
        }
        #endregion
        #region METHODS

        /// <summary>
		/// This method takes a update globalconfiguration Emails
		/// </summary>
		/// <param name="globalEmailsModel"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(List<GlobalConfigurationEmails> globalConfigurationEmails)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationEmailsService.CreateOrUpdateGlobalConfigurationEmails(globalConfigurationEmails);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });

        }
        /// <summary>
		/// This method takes a get globalconfiguration email list
		/// </summary>
		/// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return await _globalConfigurationEmailsService.GetGlobalConfigurationEmailsDataList(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}