using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfig
{
    public class DropdownOptionRepository : IDropdownOptionRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public DropdownOptionRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// this method is used by Get method of Dropdown options controller
        /// </summary>
        /// <returns>list of global congig reasons</returns>
        public async Task<IActionResult> GetAllDropdownOptions()
        {

            var globalConfigReasons = await (from globalCongigReason in _CIRDbContext.GlobalConfigurationReasons
                                             select new GlobalConfigurationReasons()
                                             {
                                                 Id = globalCongigReason.Id,
                                                 Type = globalCongigReason.Type,
                                                 Enabled = globalCongigReason.Enabled,
                                                 Content = globalCongigReason.Content,
                                             }).ToListAsync();

            if (globalConfigReasons.Count > 0)
            {
                return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = globalConfigReasons });
            }
            else
            {
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = "No Data is present" });
            }

        }


        /// <summary>
        /// This method is used by create or update method of dropdown options controller
        /// </summary>
        /// <param name="dropdownOptions"></param>
        /// <returns>return ok if successful else returns bad request</returns>
        public async Task<IActionResult> CreateOrUpdateDrownOption(List<GlobalConfigurationReasons> dropdownOptions)
        {
            if (dropdownOptions != null)
            {
                foreach (var option in dropdownOptions)
                {
                    GlobalConfigurationReasons dropdown = new GlobalConfigurationReasons()
                    {
                        Id = option.Id,
                        Type = option.Type,
                        Enabled = option.Enabled,
                        Content = option.Content,
                    };

                    if (option.Id > 0)
                    {
                        _CIRDbContext.GlobalConfigurationReasons.Update(dropdown);
                    }
                    else
                    {
                        _CIRDbContext.GlobalConfigurationReasons.Add(dropdown);
                    }
                }
                await _CIRDbContext.SaveChangesAsync();
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "DropDown Option created or updated Successfully" });
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });

        }

        #endregion
    }
}
