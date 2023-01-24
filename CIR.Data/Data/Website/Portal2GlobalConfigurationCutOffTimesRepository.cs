﻿using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.Website
{
    public class Portal2GlobalConfigurationCutOffTimesRepository : IPortal2GlobalConfigurationCutOffTimesRepository
    {

        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion
        #region CONSTRUCTOR
        public Portal2GlobalConfigurationCutOffTimesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method used by Portal2GlobalConfigurationCutOffTimes list
        /// </summary>
        /// <param name="PortalId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetPortalToGlobalConfigurationCutOffTimesList(long PortalId)
        {
            try
            {
                var result = (from PGCCutOffTimes in _CIRDBContext.Portal2GlobalConfigurationCutOffTimes
                              select new Portal2GlobalConfigurationCutOffTimesModel()
                              {
                                  Id = PGCCutOffTimes.Id,
                                  PortalId = PGCCutOffTimes.PortalId,
                                  GlobalConfigurationCutOffTimeId = PGCCutOffTimes.GlobalConfigurationCutOffTimeId,
                                  CutOffDayOverride = PGCCutOffTimes.CutOffDayOverride,
                                  CutOffTimeOverride=PGCCutOffTimes.CutOffTimeOverride.ToString()
                              }).Where(x =>

                              x.PortalId == PortalId).ToList();


                if (result != null)
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCutOffTimesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = result });
                else
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCutOffTimesModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
		/// This method is used by update method of portal2GlobalConfigurationCutOffTimes controller
		/// </summary>
		/// <param name="portal2GlobalConfigurationCutOffTimes"></param>
		/// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCutOffTimes(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes)
        {
            try
            {
                if (portal2GlobalConfigurationCutOffTimes.Any(x => x.PortalId == 0))
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                }
                if (portal2GlobalConfigurationCutOffTimes != null)
                {
                    foreach (var item in portal2GlobalConfigurationCutOffTimes)
                    {

                        var cutOffTimesCheckData = _CIRDBContext.Portal2GlobalConfigurationCutOffTimes.FirstOrDefault(x=>x.Id== item.Id);
                        Portal2GlobalConfigurationCutOffTimes cutOffTimes = new Portal2GlobalConfigurationCutOffTimes();
                        if (cutOffTimesCheckData != null)
                        {
                            cutOffTimes.CutOffDayOverride = item.CutOffDayOverride;
                            cutOffTimes.CutOffTimeOverride = TimeSpan.Parse(item.CutOffTimeOverride);
                            _CIRDBContext.Portal2GlobalConfigurationCutOffTimes.Update(cutOffTimes);
                        }
                        else
                        {
                            cutOffTimes.PortalId = item.PortalId;
                            cutOffTimes.GlobalConfigurationCutOffTimeId = item.GlobalConfigurationCutOffTimeId;
                            cutOffTimes.CutOffDayOverride = item.CutOffDayOverride;
                            cutOffTimes.CutOffTimeOverride = TimeSpan.Parse(item.CutOffTimeOverride);
                            _CIRDBContext.Portal2GlobalConfigurationCutOffTimes.Add(cutOffTimes);
                        }
                    }
                    await _CIRDBContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCutOffTimesModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
                }
                return new JsonResult(new CustomResponse<Portal2GlobalConfigurationCutOffTimesModel>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
