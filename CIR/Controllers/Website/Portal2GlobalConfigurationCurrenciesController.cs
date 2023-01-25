using CIR.Common.Enums;
using CIR.Common.Helper;
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
		/// <param name="portal2GlobalConfigurationCurrency"></param>
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
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
            }
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });

		}
		/// <summary>
		/// This method takes a get website portalToGlobalConfigurationCurrencies list
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		[HttpGet("{portalId}")]
        public async Task<IActionResult> Get(long portalId)
        {
            try
            {
                return await _portal2GlobalConfigurationCurrenciesService.GetPortalToGlobalConfigurationCurrenciesList(portalId);
            }
            catch (Exception ex)
            {
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
        }
        #endregion
    }
}
