using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
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
        /// This method get a culture list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCulture")]
        public async Task<IActionResult> GetCulture()
        {
            try
            {
                var cultureList = await _commonService.GetCultures();
                return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = cultureList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method get a site or section list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSite")]
        public async Task<IActionResult> GetSite()
        {
            try
            {
                var siteList = await _commonService.GetSite();
                return new JsonResult(new CustomResponse<List<SubSite>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = siteList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method get a role privileges list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRolePrivileges")]
        public async Task<IActionResult> GetRolePrivileges()
        {
            try
            {
                var rolePriviledgesList = await _commonService.GetRolePrivileges();
                return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = rolePriviledgesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
