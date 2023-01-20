using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationReasonsRepository : IGlobalConfigurationReasonsRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDbContext;

        #endregion

        #region CONSTRUCTORS

        public GlobalConfigurationReasonsRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method takes a get globalconfiguration reasons list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationReasons()
        {
            try
            {
                var globalConfigReasonsList = await (from globalConfigReason in _CIRDbContext.GlobalConfigurationReasons
                                                     select new GlobalConfigurationReasons()
                                                     {
                                                         Id = globalConfigReason.Id,
                                                         Type = globalConfigReason.Type,
                                                         Enabled = globalConfigReason.Enabled,
                                                         Content = globalConfigReason.Content,
                                                     }).OrderBy(x => x.Type).ThenBy(x => x.Content).ToListAsync();

                if (globalConfigReasonsList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }

                return new JsonResult(new CustomResponse<List<GlobalConfigurationReasons>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = globalConfigReasonsList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }


        /// <summary>
        /// This method takes a create or update globalconfiguration reasons
        /// </summary>
        /// <param name="globalConfigurationReasons"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationReasons(List<GlobalConfigurationReasons> globalConfigurationReasons)
        {
            try
            {
                if (globalConfigurationReasons != null)
                {
                    foreach (var option in globalConfigurationReasons)
                    {
                        GlobalConfigurationReasons globalReasons = new GlobalConfigurationReasons()
                        {
                            Id = option.Id,
                            Type = option.Type,
                            Enabled = option.Enabled,
                            Content = option.Content,
                        };

                        if (option.Id > 0)
                        {
                            _CIRDbContext.GlobalConfigurationReasons.Update(globalReasons);
                        }
                        else
                        {
                            _CIRDbContext.GlobalConfigurationReasons.Add(globalReasons);
                        }
                    }
                    await _CIRDbContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration Reason saved successfully" });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
