using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationCutOffTimesService
    {
        Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId);
        Task<IActionResult> CreateOrUpdateGlobalConfigurationCutOffTime(GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel);
    }
}
