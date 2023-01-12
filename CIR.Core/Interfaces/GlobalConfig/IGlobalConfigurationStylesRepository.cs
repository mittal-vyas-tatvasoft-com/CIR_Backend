using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationStylesRepository
    {
        Task<IActionResult> GetGlobalConfigurationStyles();
        Task<IActionResult> UpdateGlobalConfigurationStyles(List<GlobalConfigurationStyle> globalConfigurationStyles);
    }
}
