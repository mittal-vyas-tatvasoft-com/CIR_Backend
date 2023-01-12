﻿using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GlobalConfigurationCutOffTimesController : ControllerBase
    {
        #region PROPERTIES
        private readonly IGlobalConfigurationCutOffTimesService _globalConfigurationCutOffTimesService;
        #endregion

        #region CONSTRUCTOR 
        public GlobalConfigurationCutOffTimesController(IGlobalConfigurationCutOffTimesService globalConfigurationCutOffTimesService)
        {
            _globalConfigurationCutOffTimesService = globalConfigurationCutOffTimesService;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns> 

        [HttpGet("{countryId}")]
        public async Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId)
        {
            try
            {
                return await _globalConfigurationCutOffTimesService.GetGlobalConfigurationCutOffTimeByCountryWise(countryId);

            }
            catch
            {
                return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError });
            }
        }


        /// <summary>
        /// This method takes Cut Off Times details as request and save data.
        /// </summary>
        /// <param name="CutOffTime"> this object contains different parameters as details of a CutOffTime </param>
        /// <returns > success </returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationCutOffTimesService.CreateOrUpdateGlobalConfigurationCutOffTime(globalConfigurationCutOffTimeModel);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }

            }

            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _globalConfigurationCutOffTimesService.CreateOrUpdateGlobalConfigurationCutOffTime(globalConfigurationCutOffTimeModel);
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