using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationCurrenciesService : IGlobalConfigurationCurrenciesService
    {
        private readonly IGlobalConfigurationCurrenciesRepository _globalConfigurationCurrenciesRepository;
        public GlobalConfigurationCurrenciesService(IGlobalConfigurationCurrenciesRepository globalConfigurationCurrenciesRepository)
        {
            _globalConfigurationCurrenciesRepository = globalConfigurationCurrenciesRepository;
        }

        public async Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId)
        {
            return await _globalConfigurationCurrenciesRepository.GetGlobalConfigurationCurrenciesCountryWise(countryId);
        }

        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            return await _globalConfigurationCurrenciesRepository.CreateOrUpdateGlobalConfigurationCurrencies(globalCurrencyModel);
        }
    }
}
