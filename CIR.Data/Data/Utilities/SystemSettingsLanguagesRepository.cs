using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.Utilities
{
    public class SystemSettingsLanguagesRepository : ISytemSettingsLanguagesRepository
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
        /// <param name="cultureList"> List of Culture</param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateSystemSettingsLanguage(List<CulturesModel> cultureList)
        {
            try
            {
                var cultureData = GetListForUpdatedLanguages(cultureList);
                if (cultureData != null)
                {

                    foreach (Culture item in cultureData)
                    {
                        _CIRDbContext.Cultures.Update(item);
                        await _CIRDbContext.SaveChangesAsync();
                    }

                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Language Updated successfully" });
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

        /// <summary>
        /// This method is used for Getting Updated List for Languages
        /// </summary>
        /// <param name="culturesModels" ></param>
        /// <returns></returns>
        public List<Culture> GetListForUpdatedLanguages(List<CulturesModel> culturesModels)
        {
            List<Culture> list = new List<Culture>();
            foreach (CulturesModel item in culturesModels)
            {
                var culture = _CIRDbContext.Cultures.FirstOrDefault(x => x.Id == item.Id);
                if (culture != null)
                {
                    culture.Enabled = item.Enabled;
                    list.Add(culture);
                }
            }
            return list;

        }

        #endregion

    }
}
