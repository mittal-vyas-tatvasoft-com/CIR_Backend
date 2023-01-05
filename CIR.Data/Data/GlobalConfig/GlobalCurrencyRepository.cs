using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;

namespace CIR.Data.Data.GlobalConfig
{
    public class GlobalCurrencyRepository : IGlobalCurrencyRepository
    {
        private readonly CIRDbContext _CIRDBContext;
        public GlobalCurrencyRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
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
        public string CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            if (globalCurrencyModel.Any(x => x.CountryId == 0))
            {
                return "Failure";
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
                return "Success";
            }
            return "Failure";
        }
    }
}
