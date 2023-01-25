using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfiguration
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalConfigurationWeekendsController : Controller
    {
        #region PROPERTIES
        private readonly IGlobalConfigurationWeekendsService _globalConfigurationWeekendsService;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationWeekendsController(IGlobalConfigurationWeekendsService globalConfigurationWeekendsService)
        {
            _globalConfigurationWeekendsService = globalConfigurationWeekendsService;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method takes user details as parameters and creates Weekends and returns that user
        /// </summary>
        /// <param name="weekend"> this object contains different parameters as details of a weekends </param>
        /// <returns > created Weekends </returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] GlobalConfigurationWeekends weekend)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationWeekendsService.CreateGlobalConfigurationWeekendsWeekends(weekend);
                }
                catch (Exception ex)
                {
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
            }
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}


        /// <summary>
        /// This method disables Weekend 
        /// </summary>
        /// <param name="id"> Weekend will be disabled according to this id </param>
        /// <returns> disabled Weekend </returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationWeekendsService.DeleteGlobalConfigurationWeekend(id);

                }
                catch (Exception ex)
                {
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
            }
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}


        /// <summary>
        /// This method retuns filtered Weekends list using LINQ
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
        /// <param name="filterCountryCodeId">this will filter weekends with given country code id</param>
        /// <param name="filterCountryNameId">this will filter weekends with given country name id</param>
        /// <returns> filtered list of Weekends </returns>
        [HttpGet]
        public async Task<IActionResult> Get(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true)
        {
            try
            {
                search ??= string.Empty;

                return await _globalConfigurationWeekendsService.GetGlobalConfigurationWeekends(displayLength, displayStart, sortCol, filterCountryNameId, filterCountryCodeId, search, sortAscending);
            }
            catch (Exception ex)
            {
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
        }
        #endregion
    }
}
