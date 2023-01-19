using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.Website
{
    public class PortalToGlobalConfigurationCurrenciesRepository :IPortalToGlobalConfigurationCurrenciesRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion
        #region CONSTRUCTOR
        public PortalToGlobalConfigurationCurrenciesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion
        #region METHODS
        /// <summary>
        /// This method used by PortalToGlobalConfigurationCurrencies list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(int id)
        {
            try
            {
                var result = (from PGCurrency in _CIRDBContext.Portal2GlobalConfigurationCurrencies
                              select new PortalToGlobalConfigurationCurrency()
                              {
                                  Id = PGCurrency.Id,
                                  PortalId = PGCurrency.PortalId,
                                  GlobalConfigurationCurrencyId = PGCurrency.GlobalConfigurationCurrencyId,
                                  EnabledOverride = PGCurrency.EnabledOverride,
                                  

                              }).Where(x =>
                              
                              x.PortalId == id).ToList();

                
                var CurrencyId = (from PGCC in _CIRDBContext.Portal2GlobalConfigurationCurrencies
                                  select new PortalToGlobalConfigurationCurrency()
                                  {
                                      Id = PGCC.Id,
                                      PortalId = PGCC.PortalId,
                                      GlobalConfigurationCurrencyId = PGCC.GlobalConfigurationCurrencyId,
                                      EnabledOverride = PGCC.EnabledOverride
                                  }).FirstOrDefault(x => x.PortalId == id);
                var serializedParent = JsonConvert.SerializeObject(CurrencyId);
                PortalToGlobalConfigurationCurrency Currency = JsonConvert.DeserializeObject<PortalToGlobalConfigurationCurrency>(serializedParent);

                if (Currency.EnabledOverride)
                {
                    Currency.EnabledOverride = true;
                }
               
                if (result != null)
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = Result });
                else
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
		/// This method is used by create method and update method of portalToGlobalConfigurationCurrencies controller
		/// </summary>
		/// <param name="portalToGlobalConfigurationCurrencies"></param>
		/// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<PortalToGlobalConfigurationCurrency> portalToGlobalConfigurationCurrencies)
        {
            try
            {
                
                if (portalToGlobalConfigurationCurrencies != null)
                {
                    foreach (var item in portalToGlobalConfigurationCurrencies)
                    {
                            var Currency = _CIRDBContext.Portal2GlobalConfigurationCurrencies.FirstOrDefault(x => x.Id == item.Id);
                            if (Currency != null)
                            {
                                Currency.PortalId = item.PortalId;
                                Currency.GlobalConfigurationCurrencyId = item.GlobalConfigurationCurrencyId;
                                Currency.EnabledOverride = item.EnabledOverride;
                                
                            }
                            else
                            {
                                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                            }
                        
                            _CIRDBContext.Portal2GlobalConfigurationCurrencies.Update(Currency);
                    }
                    await _CIRDBContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<List<PortalToGlobalConfigurationEmails>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
                }
                return new JsonResult(new CustomResponse<PortalToGlobalConfigurationEmails>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
