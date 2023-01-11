using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalCurrencyService
    {
        List<GlobalConfigurationCurrencyModel> GetCurrencyCountryWise(int countryId);
        Task<IActionResult> CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel);
    }
}
