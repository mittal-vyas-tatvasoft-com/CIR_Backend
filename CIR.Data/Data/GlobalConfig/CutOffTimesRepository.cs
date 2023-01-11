﻿using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.GlobalConfig;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.GlobalConfig
{
    public class CutOffTimesRepository : ControllerBase,ICutOffTimesRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _CIRDBContext;
        private readonly IConfiguration Configuration;
        #endregion

        #region CONSTRUCTOR
        public CutOffTimesRepository(CIRDbContext context, IConfiguration _configuration)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));

            Configuration = _configuration;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// fetches Cut Off Time and Day based on input Country id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Cut Off Time and Day</returns>

        public async Task<IActionResult> GetCutOffTimeAndDayById(int id)
        {
            var checkUserExist = await _CIRDBContext.GlobalConfigurationCutOffTimes.Where(x => x.CountryId == id).FirstOrDefaultAsync();

            if (checkUserExist != null)
            {
                GlobalConfigurationCutOffTimeModel CutOffTimeAndDay = new()
                {
                    Id= checkUserExist.Id,
                    CountryId = checkUserExist.CountryId,
                    CutOffTime = checkUserExist.CutOffTime.ToString(),
                    CutOffDay = checkUserExist.CutOffDay
                };
                return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success,Data=CutOffTimeAndDay});
            }
            else
            {
                GlobalConfigurationCutOffTimeModel CutOffTimeAndDay = new()
                {
                    CountryId = id,
                    CutOffTime = this.Configuration.GetSection("StaticCutOffTime")["CutOffTime"],
                    CutOffDay = (int)CutOffDays.PreviousDay
                };
                return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTimeModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = CutOffTimeAndDay });
            }
           
           
        }

        /// <summary>
        /// This method is used by save method of Cut Off Time controller
        /// </summary>
        /// <param name="model"> save CutOffTime data </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> SaveCutOffTime (GlobalConfigurationCutOffTimeModel model)
        {
            try
            {
                GlobalConfigurationCutOffTime newCutOffTime = new()
                {
                    Id = model.Id,

                    CountryId = model.CountryId,

                    CutOffTime = TimeSpan.Parse(model.CutOffTime),

                    CutOffDay = model.CutOffDay

                };
                if (model.Id > 0)
                {
                    _CIRDBContext.GlobalConfigurationCutOffTimes.Update(newCutOffTime);
                }
                else
                {
                    _CIRDBContext.GlobalConfigurationCutOffTimes.Add(newCutOffTime);
                }
                             
                await _CIRDBContext.SaveChangesAsync();

                return new JsonResult(new CustomResponse<GlobalConfigurationCutOffTime>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}