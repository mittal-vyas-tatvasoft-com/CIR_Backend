using CIR.Common.CustomResponse;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Portal2GlobalConfigurationCurrenciesController : Controller
    {
        #region PROPERTIES
        private readonly IPortal2GlobalConfigurationCurrenciesService _portal2GlobalConfigurationCurrenciesService;
        #endregion

        #region CONSTRUCTORS
        public Portal2GlobalConfigurationCurrenciesController(IPortal2GlobalConfigurationCurrenciesService portal2GlobalConfigurationCurrenciesService)
        {
            _portal2GlobalConfigurationCurrenciesService = portal2GlobalConfigurationCurrenciesService;
        }
        #endregion

        #region METHODS

        /// <summary>
		/// This method takes a update website portal2GlobalConfigurationCurrencies
		/// </summary>
		/// <param name="portalToGlobalConfigurationCurrency"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portal2GlobalConfigurationCurrenciesService.UpdatePortalToGlobalConfigurationCurrencies(portal2GlobalConfigurationCurrency);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });

        }
        /// <summary>
		/// This method takes a get website portalToGlobalConfigurationCurrencies list
		/// </summary>
		/// <returns></returns>
        [HttpGet("{PortalId}")]
        public async Task<IActionResult> Get(long PortalId)
        {
            try
            {
                return await _portal2GlobalConfigurationCurrenciesService.GetPortalToGlobalConfigurationCurrenciesList(PortalId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
