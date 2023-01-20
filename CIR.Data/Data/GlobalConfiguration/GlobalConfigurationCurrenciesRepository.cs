using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationCurrenciesRepository : IGlobalConfigurationCurrenciesRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion

        #region CONSTRUCTORS
        public GlobalConfigurationCurrenciesRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region METHODS

        /// <summary>
        /// This method used by getcurrency List countryid wise
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId)
        {
            try
            {
                var globalConfigurationCurrenciesList = await (from globalCurrency in _CIRDBContext.GlobalConfigurationCurrencies
                                                               join country in _CIRDBContext.CountryCodes
                                                               on globalCurrency.CountryId equals country.Id
                                                               join currency in _CIRDBContext.Currencies
                                                               on globalCurrency.CurrencyId equals currency.Id
                                                               select new GlobalConfigurationCurrencyModel()
                                                               {
                                                                   Id = globalCurrency.Id,
                                                                   CountryId = globalCurrency.CountryId,
                                                                   CurrencyId = globalCurrency.CurrencyId,
                                                                   Enabled = globalCurrency.Enabled,
                                                                   CountryName = country.CountryName,
                                                                   CodeName = currency.CodeName
                                                               }).Where(x => x.CountryId == countryId).ToListAsync();

                if (globalConfigurationCurrenciesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
                }
                return new JsonResult(new CustomResponse<List<GlobalConfigurationCurrencyModel>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = globalConfigurationCurrenciesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method is used by create method and update method of globalcurrency controller
        /// </summary>
        /// <param name="globalCurrencyModel"></param>
        /// <returns>Success status if its valid else failure</returns>

        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            try
            {
                if (globalCurrencyModel.Any(x => x.CountryId == 0 || x.CurrencyId == 0))
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid Data" });
                }
                if (globalCurrencyModel != null)
                {
                    foreach (var item in globalCurrencyModel)
                    {
                        bool globalConfigurationCurrenciesDuplicate = _CIRDBContext.GlobalConfigurationCurrencies.Any(x => x.CountryId == item.CountryId && x.CurrencyId == item.CurrencyId && x.Id != item.Id);

                        if (!globalConfigurationCurrenciesDuplicate)
                        {
                            GlobalConfigurationCurrency currency = new GlobalConfigurationCurrency()
                            {
                                Id = item.Id,
                                CountryId = item.CountryId,
                                CurrencyId = item.CurrencyId,
                                Enabled = item.Enabled
                            };
                            _CIRDBContext.GlobalConfigurationCurrencies.Update(currency);
                        }
                        else
                        {
                            GlobalConfigurationCurrency currency = new GlobalConfigurationCurrency()
                            {
                                CountryId = item.CountryId,
                                CurrencyId = item.CurrencyId,
                                Enabled = item.Enabled
                            };
                            _CIRDBContext.GlobalConfigurationCurrencies.Add(currency);
                        }
                    }
                    await _CIRDBContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Global Currency saved successfully" });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
