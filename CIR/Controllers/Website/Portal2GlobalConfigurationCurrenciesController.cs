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
        private readonly IPortal2GlobalConfigurationCurrenciesService _portalToGlobalConfigurationCurrenciesService;
        #endregion

        #region CONSTRUCTORS
        public Portal2GlobalConfigurationCurrenciesController(IPortal2GlobalConfigurationCurrenciesService portalToGlobalConfigurationCurrenciesService)
        {
            _portalToGlobalConfigurationCurrenciesService = portalToGlobalConfigurationCurrenciesService;
        }
        #endregion

        #region METHODS

        /// <summary>
		/// This method takes a update website portalToGlobalConfigurationCurrencies
		/// </summary>
		/// <param name="portalToGlobalConfigurationEmails"></param>
		/// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Post(List<Portal2GlobalConfigurationCurrency> portalToGlobalConfigurationCurrency)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _portalToGlobalConfigurationCurrenciesService.UpdatePortalToGlobalConfigurationCurrencies(portalToGlobalConfigurationCurrency);

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });

        }
        /// <summary>
		/// This method takes a get website portalToGlobalConfigurationEmails list
		/// </summary>
		/// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return await _portalToGlobalConfigurationCurrenciesService.GetPortalToGlobalConfigurationCurrenciesList(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
