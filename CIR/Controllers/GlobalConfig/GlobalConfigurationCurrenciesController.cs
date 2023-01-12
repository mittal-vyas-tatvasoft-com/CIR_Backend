using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalConfigurationCurrenciesController : ControllerBase
    {
        #region PROPERTIES

        private readonly IGlobalConfigurationCurrenciesService _globalConfigurationCurrenciesService;

        #endregion

        #region CONSTRUCTORS

        public GlobalConfigurationCurrenciesController(IGlobalConfigurationCurrenciesService globalConfigurationCurrenciesService)
        {
            _globalConfigurationCurrenciesService = globalConfigurationCurrenciesService;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method takes get global currency country wise
        /// </summary>
        /// <param name="countryId">this object contains countryId</param>
        /// <returns></returns>
        [HttpGet("{countryId}")]
        public async Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId)
        {
            try
            {
                return await _globalConfigurationCurrenciesService.GetGlobalConfigurationCurrenciesCountryWise(countryId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes add global currency
        /// </summary>
        /// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationCurrenciesService.CreateOrUpdateGlobalConfigurationCurrencies(globalCurrencyModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
        }

        /// <summary>
        /// This method takes update global currency
        /// </summary>
        /// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationCurrenciesService.CreateOrUpdateGlobalConfigurationCurrencies(globalCurrencyModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }

            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
        }
        #endregion
    }
}
