using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Utilities.SystemSettings;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.Utilities.SystemSettings
{
    public class SystemSettingsLanguagesRepository :ISytemSettingsLanguagesRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _CIRDbContext;
        #endregion

        #region CONSTRUCTOR
        public SystemSettingsLanguagesRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method is used by Update method of SystemSettings Languages
        /// </summary>
        /// <param name="culture"> Update Languages data</param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateSystemSettingsLanguage(CulturesModel culture)
        {
            try
            {
                var cultureeData = _CIRDbContext.Cultures.FirstOrDefault(x => x.Id == culture.Id);
                if(cultureeData != null)
                {
                    Culture _culture = cultureeData;

                    _culture.Enabled = culture.Enabled;
                   _CIRDbContext.Cultures.Update(_culture);
                    await _CIRDbContext.SaveChangesAsync();

                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated,Data= "Language Updated successfully" });
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = true, Message = HttpStatusCodesMessages.NotFound, Data = "Language id not found." });
                }
             
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = ex });
            }
        }

        #endregion

    }
}
