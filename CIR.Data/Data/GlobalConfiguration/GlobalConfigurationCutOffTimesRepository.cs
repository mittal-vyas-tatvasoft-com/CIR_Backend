using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.GlobalConfiguration;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationCutOffTimesRepository : ControllerBase, IGlobalConfigurationCutOffTimesRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _cIRDbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationCutOffTimesRepository(CIRDbContext cIRDbContext, IConfiguration configuration)
        {
            _cIRDbContext = cIRDbContext ??
               throw new ArgumentNullException(nameof(cIRDbContext));

            _configuration = configuration;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method takes a get globalconfiguration cuttoftime countrywise
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>

        public async Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId)
        {
            try
            {
                var checkCountryIdData = await _cIRDbContext.GlobalConfigurationCutOffTimes.Where(x => x.CountryId == countryId).FirstOrDefaultAsync();

                if (checkCountryIdData != null)
                {
                    GlobalConfigurationCutOffTimeModel cutOffTime = new()
                    {
                        Id = checkCountryIdData.Id,
                        CountryId = checkCountryIdData.CountryId,
                        CutOffTime = checkCountryIdData.CutOffTime.ToString(),
                        CutOffDay = checkCountryIdData.CutOffDay
                    };
                    return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = cutOffTime });
                }
                else
                {
                    GlobalConfigurationCutOffTimeModel cutOffTimeModel = new()
                    {
                        CountryId = countryId,
                        CutOffTime = _configuration.GetSection("StaticCutOffTime").GetSection("CutOffTime").Value,
                        CutOffDay = (int)CutOffDays.PreviousDay
                    };
                    return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = cutOffTimeModel });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes a create or update globalconfiguration cutofftime
        /// </summary>
        /// <param name="globalConfigurationCutOffTimeModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationCutOffTime(GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel)
        {
            try
            {
                if (globalConfigurationCutOffTimeModel.CutOffTime == null || globalConfigurationCutOffTimeModel.CutOffTime == "string" || globalConfigurationCutOffTimeModel.CountryId == 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid data" });
                }
                GlobalConfigurationCutOffTime newCutOffTime = new()
                {
                    Id = globalConfigurationCutOffTimeModel.Id,

                    CountryId = globalConfigurationCutOffTimeModel.CountryId,

                    CutOffTime = TimeSpan.Parse(globalConfigurationCutOffTimeModel.CutOffTime),

                    CutOffDay = globalConfigurationCutOffTimeModel.CutOffDay
                };

                if (globalConfigurationCutOffTimeModel.Id > 0)
                {
                    _cIRDbContext.GlobalConfigurationCutOffTimes.Update(newCutOffTime);
                }
                else
                {
                    _cIRDbContext.GlobalConfigurationCutOffTimes.Add(newCutOffTime);
                }

                await _cIRDbContext.SaveChangesAsync();

                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "GlobalConfiguration CutOfTime saved successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
