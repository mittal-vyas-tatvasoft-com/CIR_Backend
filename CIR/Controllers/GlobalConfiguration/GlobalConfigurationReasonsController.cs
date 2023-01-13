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
    public class GlobalConfigurationReasonsController : ControllerBase
    {
        #region PROPERTIES

        private readonly IGlobalConfigurationReasonsService _globalConfigurationReasonsService;

        #endregion

        #region CONSTRUCTORS

        public GlobalConfigurationReasonsController(IGlobalConfigurationReasonsService globalConfigurationReasonsService)
        {
            _globalConfigurationReasonsService = globalConfigurationReasonsService;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// This method takes a get globalconfiguration reasons list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetGlobalConfigurationReasons()
        {
            try
            {
                return await _globalConfigurationReasonsService.GetGlobalConfigurationReasons();
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes a create globalconfiguration reason
        /// </summary>
        /// <param name="globalConfigurationReasons"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(List<GlobalConfigurationReasons> globalConfigurationReasons)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationReasonsService.CreateOrUpdateGlobalConfigurationReasons(globalConfigurationReasons);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }


        /// <summary>
        /// This method takes a update globalconfiguration reason
        /// </summary>
        /// <param name="globalConfigurationReasons"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(List<GlobalConfigurationReasons> globalConfigurationReasons)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationReasonsService.CreateOrUpdateGlobalConfigurationReasons(globalConfigurationReasons);
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
