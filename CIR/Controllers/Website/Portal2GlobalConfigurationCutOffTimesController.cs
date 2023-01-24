using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Portal2GlobalConfigurationCutOffTimesController : Controller
    {
        #region PROPERTIES
        private readonly IPortal2GlobalConfigurationCutOffTimesService _portal2GlobalConfigurationCutOffTimesService;
        #endregion

        #region CONSTRUCTORS
        public Portal2GlobalConfigurationCutOffTimesController(IPortal2GlobalConfigurationCutOffTimesService portal2GlobalConfigurationCutOffTimesService)
        {
            _portal2GlobalConfigurationCutOffTimesService = portal2GlobalConfigurationCutOffTimesService;
        }
        #endregion

        #region METHODS

        /// <summary>
		/// This method takes a update website portal2GlobalConfigurationCutOffTimes
		/// </summary>
		/// <param name="portalToGlobalConfigurationCutOffTimes"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portal2GlobalConfigurationCutOffTimesService.UpdatePortalToGlobalConfigurationCutOffTimes(portal2GlobalConfigurationCutOffTimes);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });

        }
        /// <summary>
		/// This method takes a get website portalToGlobalConfigurationCutOffTimes list
		/// </summary>
		/// <returns></returns>
        [HttpGet("{PortalId}")]
        public async Task<IActionResult> Get(long PortalId)
        {
            try
            {
                return await _portal2GlobalConfigurationCutOffTimesService.GetPortalToGlobalConfigurationCutOffTimesList(PortalId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
