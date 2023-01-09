using CIR.Application.Services.Users;
using CIR.Common.CustomResponse;
using CIR.Controllers.Users;
using CIR.Core.Entities;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CutOffTimesController : ControllerBase
    {
        #region PROPERTIES
        private readonly ICutOffTimesService _cutOffTimesService;
        private readonly ILogger<CutOffTimesController> _logger;
        #endregion

        #region CONSTRUCTOR 
        public CutOffTimesController(ICutOffTimesService cutOffTimesService, ILogger<CutOffTimesController> logger)
        {
            _cutOffTimesService = cutOffTimesService;
            _logger = logger;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns> 

        [HttpGet("{id}")]
        public async Task<CustomResponse<GlobalConfigurationCutOffTimeModel>> GetCutOffTimeAndDayById(int id)
        {
            try
            {
                var data = await _cutOffTimesService.GetCutOffTimeAndDayById(id);
                if (data != null)
                {
                    return new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = data };
                }
                return new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = data };

            }
            catch
            {
                return new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError };
            }
        }


        /// <summary>
        /// This method takes Cut Off Times details as request and save data.
        /// </summary>
        /// <param name="CutOffTime"> this object contains different parameters as details of a CutOffTime </param>
        /// <returns > success </returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Save([FromBody] GlobalConfigurationCutOffTimeModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                        return await _cutOffTimesService.SaveCutOffTime(model);
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
