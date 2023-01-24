using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Utilities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemSettingsLanguagesController
    {
        #region PROPERTIES
        private readonly ISystemSettingsLanguagesServices _isystemSettingsLanguagesServices;
        #endregion

        #region CONSTRUCTOR
        public SystemSettingsLanguagesController(ISystemSettingsLanguagesServices systemSettingsLanguagesServices)
        {
            _isystemSettingsLanguagesServices = systemSettingsLanguagesServices;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method takes Language details as parameters and Update Selected Languages 
        /// </summary>
        /// <param name="culture"> this object contains different parameters as details of a Languages </param>
        /// <returns ></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] List<CulturesModel> culture)
        {
            try
            {
                return await _isystemSettingsLanguagesServices.UpdateSystemSettingsLanguage(culture);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
