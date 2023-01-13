using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
    public interface IGlobalConfigurationCutOffTimesRepository
    {
        Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId);
        Task<IActionResult> CreateOrUpdateGlobalConfigurationCutOffTime(GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel);
    }
}
