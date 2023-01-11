using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DropdownOptionsController : ControllerBase
    {
        #region PROPERTIES

        private readonly IDropdownOptionService _dropdownOptionService;

        #endregion

        #region CONSTRUCTORS

        public DropdownOptionsController(IDropdownOptionService dropdownOptionService)
        {
            _dropdownOptionService = dropdownOptionService;
        }

        #endregion


        #region METHODS

        /// <summary>
        /// This method fetches the list of dropdown options
        /// </summary>
        /// <returns>list of dropdown option></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return await _dropdownOptionService.GetAllDropdownOptions();
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes dropdown option detail and adds it
        /// </summary>
        /// <param name="dropdownOption">this object contains different parameters as details of a dropdown option</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(List<GlobalConfigurationReasons> dropdownOptions)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _dropdownOptionService.CreateOrUpdateDrownOption(dropdownOptions);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }


        /// <summary>
        /// This method takes dropdown option detail and updates it
        /// </summary>
        /// <param name="dropdownOption">this object contains different parameters as details of a dropdown option</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update(List<GlobalConfigurationReasons> dropdownOptions)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _dropdownOptionService.CreateOrUpdateDrownOption(dropdownOptions);
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
