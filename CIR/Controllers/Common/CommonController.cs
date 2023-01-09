using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : ControllerBase
    {
        #region PROPERTIES

        private readonly ICommonService _commonService;

        #endregion

        #region CONSTRUCTORS

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method get a currencies list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currencyList = _commonService.GetCurrencies();
                return new JsonResult(new CustomResponse<List<Currency>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = currencyList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });

            }

        }

        /// <summary>
        /// This method get a country list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountry")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var countryList = _commonService.GetCountry();
                return new JsonResult(new CustomResponse<List<CountryCode>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = countryList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });

            }

        }

        /// <summary>
        /// This method return the list of cultures
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCultures")]
        public async Task<IActionResult> GetCultures()
        {
            try
            {
                var cultures = _commonService.GetCultures();
                if (cultures.Count > 0)
                {
                    return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = cultures });
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = "No data present" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
