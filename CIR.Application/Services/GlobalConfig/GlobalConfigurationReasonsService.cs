using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
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
