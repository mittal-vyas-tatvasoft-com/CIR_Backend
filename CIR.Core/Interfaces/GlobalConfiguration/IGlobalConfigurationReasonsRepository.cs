using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
    public interface IGlobalConfigurationReasonsRepository
    {
        Task<IActionResult> GetGlobalConfigurationReasons();
        Task<IActionResult> CreateOrUpdateGlobalConfigurationReasons(List<GlobalConfigurationReasons> globalConfigurationReasons);
    }
}
