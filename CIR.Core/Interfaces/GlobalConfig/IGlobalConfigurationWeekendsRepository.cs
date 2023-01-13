using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationWeekendsRepository
    {
        Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends globalConfigurationWeekends);
        Task<IActionResult> DeleteGlobalConfigurationWeekend(int id);
        Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true);
    }
}
