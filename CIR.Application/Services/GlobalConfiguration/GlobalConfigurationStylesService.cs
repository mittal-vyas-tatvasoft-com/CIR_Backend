using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
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
