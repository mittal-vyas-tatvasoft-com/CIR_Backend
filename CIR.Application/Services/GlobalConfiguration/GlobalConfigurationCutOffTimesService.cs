using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
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
