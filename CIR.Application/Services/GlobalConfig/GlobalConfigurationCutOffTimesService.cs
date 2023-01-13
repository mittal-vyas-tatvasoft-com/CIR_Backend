using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalConfigurationCutOffTimesService : IGlobalConfigurationCutOffTimesService
    {
        private readonly IGlobalConfigurationCutOffTimesRepository _globalConfigurationCutOffTimesService;

        public GlobalConfigurationCutOffTimesService(IGlobalConfigurationCutOffTimesRepository globalConfigurationCutOffTimesService)
        {
            _globalConfigurationCutOffTimesService = globalConfigurationCutOffTimesService;
        }
        public async Task<IActionResult> GetGlobalConfigurationCutOffTimeByCountryWise(int countryId)
        {
            return await _globalConfigurationCutOffTimesService.GetGlobalConfigurationCutOffTimeByCountryWise(countryId);
        }
        public Task<IActionResult> CreateOrUpdateGlobalConfigurationCutOffTime(GlobalConfigurationCutOffTimeModel globalConfigurationCutOffTimeModel)
        {
            return _globalConfigurationCutOffTimesService.CreateOrUpdateGlobalConfigurationCutOffTime(globalConfigurationCutOffTimeModel);
        }

    }
}
