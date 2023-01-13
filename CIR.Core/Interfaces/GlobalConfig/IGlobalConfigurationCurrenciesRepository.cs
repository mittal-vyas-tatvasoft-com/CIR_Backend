using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationCurrenciesRepository
    {
        Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId);
        Task<IActionResult> CreateOrUpdateGlobalConfigurationCurrencies(List<GlobalCurrencyModel> globalCurrencyModel);
    }
}
