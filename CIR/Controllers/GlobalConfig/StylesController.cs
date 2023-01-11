using CIR.Application.Services.GlobalConfig;
using CIR.Application.Services.Users;
using CIR.Common.CustomResponse;
using CIR.Controllers.Users;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StylesController : ControllerBase
    {
        #region PROPERTIES
        private readonly IStylesService _styleService;
        #endregion

        #region CONSTRUCTOR
        public StylesController(IStylesService styleService)
        {
            _styleService = styleService;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method retuns Style list using LINQ
        /// </summary>
        /// <returns> list of style </returns>

        [HttpGet]
        public async Task<IActionResult> StylesList()
        {
            try
            {
                var styleData = await _styleService.GetAllStyles();
            
                return new JsonResult(new CustomResponse<GlobalConfigurationStyleModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = styleData });
               
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
        /// This method takes Style details as request and save data.
        /// </summary>
        /// <param name="Style"> this object contains different parameters as details of a Style </param>
        /// <returns > success </returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Save([FromBody] List<GlobalConfigurationStyle> model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _styleService.SaveStyle(model);
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
