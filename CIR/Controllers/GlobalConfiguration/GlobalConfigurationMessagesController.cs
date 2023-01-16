using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalConfigurationMessagesController : ControllerBase
    {
        #region PROPERTIES

        private readonly IGlobalConfigurationMessagesService _globalConfigurationMessagesService;

        #endregion

        #region CONSTRUCTORS

        public GlobalConfigurationMessagesController(IGlobalConfigurationMessagesService globalConfigurationMessagesService)
        {
            _globalConfigurationMessagesService = globalConfigurationMessagesService;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method takes a get globalconfiguration messages list
        /// </summary>
        /// <param name="cultureId"></param>
        /// <returns></returns>
        [HttpGet("{cultureId}")]
        public async Task<IActionResult> GetGlobalConfigurationMessagesList(int cultureId)
        {
            try
            {
                return await _globalConfigurationMessagesService.GetGlobalConfigurationMessagesList(cultureId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes a create globalconfiguration messages
        /// </summary>
        /// <param name="globalConfigurationMessages"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(List<GlobalConfigurationMessages> globalConfigurationMessages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationMessagesService.CreateOrUpdateGlobalConfigurationMessages(globalConfigurationMessages);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        /// <summary>
        /// This method takes a update globalconfiguration messages
        /// </summary>
        /// <param name="globalConfigurationMessages"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(List<GlobalConfigurationMessages> globalConfigurationMessages)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationMessagesService.CreateOrUpdateGlobalConfigurationMessages(globalConfigurationMessages);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }
        #endregion
    }
}
