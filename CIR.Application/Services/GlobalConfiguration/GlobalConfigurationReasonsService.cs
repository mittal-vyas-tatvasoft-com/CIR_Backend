using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationReasonsService : IGlobalConfigurationReasonsService
    {
        private readonly IGlobalConfigurationReasonsRepository _globalConfigurationReasonsRepository;

        public GlobalConfigurationReasonsService(IGlobalConfigurationReasonsRepository globalConfigurationReasonsRepository)
        {
            _globalConfigurationReasonsRepository = globalConfigurationReasonsRepository;
        }

        public async Task<IActionResult> GetGlobalConfigurationReasons()
        {
            return await _globalConfigurationReasonsRepository.GetGlobalConfigurationReasons();
        }

        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationReasons(List<GlobalConfigurationReasons> globalConfigurationReasons)
        {
            return await _globalConfigurationReasonsRepository.CreateOrUpdateGlobalConfigurationReasons(globalConfigurationReasons);
        }
    }
}
