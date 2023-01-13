using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalConfigurationStylesService : IGlobalConfigurationStylesService
    {
        private readonly IGlobalConfigurationStylesRepository _globalConfigurationStylesService;

        public GlobalConfigurationStylesService(IGlobalConfigurationStylesRepository globalConfigurationStylesService)
        {
            _globalConfigurationStylesService = globalConfigurationStylesService;
        }
        public Task<IActionResult> GetGlobalConfigurationStyles()
        {
            return _globalConfigurationStylesService.GetGlobalConfigurationStyles();
        }
        public Task<IActionResult> UpdateGlobalConfigurationStyles(List<GlobalConfigurationStyle> globalConfigurationStyles)
        {
            return _globalConfigurationStylesService.UpdateGlobalConfigurationStyles(globalConfigurationStyles);
        }
    }
}
