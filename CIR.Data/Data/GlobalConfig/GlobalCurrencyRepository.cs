using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Data.Data.GlobalConfig
{
    public class GlobalCurrencyRepository : IGlobalCurrencyRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion

        #region CONSTRUCTORS
        public GlobalCurrencyRepository(CIRDbContext context)
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
        public List<GlobalConfigurationCurrencyModel> GetCurrencyCountryWise(int countryId)
        {
            List<GlobalConfigurationCurrencyModel> globalConfigurationCurrenciesList = new List<GlobalConfigurationCurrencyModel>();

            globalConfigurationCurrenciesList = (from globalCurrency in _CIRDBContext.GlobalConfigurationCurrencies
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
                                                 }).Where(x => x.CountryId == countryId).ToList();

            return globalConfigurationCurrenciesList;
        }

        /// <summary>
        /// This method is used by create method and update method of globalcurrency controller
        /// </summary>
        /// <param name="globalCurrencyModel"></param>
        /// <returns>Success status if its valid else failure</returns>


        public async Task<IActionResult> CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (globalCurrencyModel.Any(x => x.CountryId == 0 || x.CurrencyId == 0))
            {
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid Data" });
            }
            if (globalCurrencyModel != null)
            {
                foreach (var item in globalCurrencyModel)
                {
                    if (item.Id != 0)
                    {
                        GlobalConfigurationCurrency curr = new GlobalConfigurationCurrency()
                        {
                            Id = item.Id,
                            CountryId = item.CountryId,
                            CurrencyId = item.CurrencyId,
                            Enabled = item.Enabled
                        };
                        _CIRDBContext.GlobalConfigurationCurrencies.Update(curr);
                    }
                    else
                    {
                        GlobalConfigurationCurrency curr = new GlobalConfigurationCurrency()
                        {
                            CountryId = item.CountryId,
                            CurrencyId = item.CurrencyId,
                            Enabled = item.Enabled
                        };
                        _CIRDBContext.GlobalConfigurationCurrencies.Add(curr);
                    }
                }
                _CIRDBContext.SaveChanges();
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Global Currency added successfully" });
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while adding new Global Currency" });
        }
        #endregion
    }
}
