using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationCutOffTimesRepository
    {
        Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId);
        Task<IActionResult> CreateOrUpdateGlobalConfigurationCutOffTime(GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel);
    }
}
