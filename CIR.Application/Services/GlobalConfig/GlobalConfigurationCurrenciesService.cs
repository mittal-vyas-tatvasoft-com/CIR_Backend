using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
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
