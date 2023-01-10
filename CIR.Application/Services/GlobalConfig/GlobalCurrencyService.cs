using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalCurrencyService : IGlobalCurrencyService
    {
        private readonly IGlobalCurrencyRepository _currencyRepository;
        public GlobalCurrencyService(IGlobalCurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public List<GlobalConfigurationCurrencyModel> GetCurrencyCountryWise(int countryId)
        {
            return _currencyRepository.GetCurrencyCountryWise(countryId);
        }

        public Task<IActionResult> CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            return _currencyRepository.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
        }
    }
}
