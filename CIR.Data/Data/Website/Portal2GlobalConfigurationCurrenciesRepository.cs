using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.Website;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIR.Data.Data.Website
{
    public class Portal2GlobalConfigurationCurrenciesRepository : IPortal2GlobalConfigurationCurrenciesRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion
        #region CONSTRUCTOR
        public Portal2GlobalConfigurationCurrenciesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion
        #region METHODS
        /// <summary>
        /// This method used by Portal2GlobalConfigurationCurrencies list
        /// </summary>
        /// <param name="PortalId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(long PortalId)
        {
            try
            {
                var result = (from PGCurrency in _CIRDBContext.Portal2GlobalConfigurationCurrencies
                              select new Portal2GlobalConfigurationCurrency()
                              {
                                  Id = PGCurrency.Id,
                                  PortalId = PGCurrency.PortalId,
                                  GlobalConfigurationCurrencyId = PGCurrency.GlobalConfigurationCurrencyId,
                                  EnabledOverride = PGCurrency.EnabledOverride,


                              }).Where(x =>

                              x.PortalId == PortalId).ToList();


                var CurrencyId = (from PGCC in _CIRDBContext.Portal2GlobalConfigurationCurrencies
                                  select new Portal2GlobalConfigurationCurrency()
                                  {
                                      Id = PGCC.Id,
                                      PortalId = PGCC.PortalId,
                                      GlobalConfigurationCurrencyId = PGCC.GlobalConfigurationCurrencyId,
                                      EnabledOverride = PGCC.EnabledOverride
                                  }).FirstOrDefault(x => x.PortalId == PortalId);
                var serializedParent = JsonConvert.SerializeObject(CurrencyId);
                Portal2GlobalConfigurationCurrency Currency = JsonConvert.DeserializeObject<Portal2GlobalConfigurationCurrency>(serializedParent);

                if (Currency.EnabledOverride)
                {
                    Currency.EnabledOverride = true;
                }

                if (result != null)
                {
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = result });
                }
                else
                {
                    return new JsonResult(new CustomResponse<List<Portal2GlobalConfigurationCurrency>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
		/// This method is used by create method and update method of portal2GlobalConfigurationCurrencies controller
		/// </summary>
		/// <param name="portal2GlobalConfigurationCurrencies"></param>
		/// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrencies)
        {
            try
            {

                if (portal2GlobalConfigurationCurrencies != null)
                {
                    foreach (var item in portal2GlobalConfigurationCurrencies)
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
