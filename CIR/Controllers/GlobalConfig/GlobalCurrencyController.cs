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
    public class GlobalCurrencyController : ControllerBase
    {
        #region PROPERTIES

        private readonly IGlobalCurrencyService _currencyService;

        #endregion

        #region CONSTRUCTORS

        public GlobalCurrencyController(IGlobalCurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method takes get global currency country wise
        /// </summary>
        /// <param name="countryId">this object contains countryId</param>
        /// <returns></returns>
        [HttpGet("{countryId}")]
        public async Task<IActionResult> Get(int countryId)
        {
            try
            {
                var currencyList = _currencyService.GetCurrencyCountryWise(countryId);
                return new JsonResult(new CustomResponse<List<GlobalConfigurationCurrencyModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = currencyList });
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
        [HttpPost]
        public async Task<IActionResult> Post(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var addGlobalCurrency = _currencyService.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
                    if (addGlobalCurrency == "Success")
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Global Currency added successfully" });
                    }
                    else if (addGlobalCurrency == "InValid Data")
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid Data" });
                    }
                    else
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });

                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }

            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        /// <summary>
        /// This method takes update global currency
        /// </summary>
        /// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updateGlobalCurrency = _currencyService.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
                    if (updateGlobalCurrency == "Success")
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Global Currency updated successfully" });
                    }
                    else if (updateGlobalCurrency == "InValid Data")
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid Data" });
                    }
                    else
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while updating Global Currency" });
                    }
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
