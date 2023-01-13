using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationWeekendsService
    {
        public Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends globalConfigurationWeekends);
        public Task<IActionResult> DeleteGlobalConfigurationWeekend(int id);
        public Task<IActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending);
    }
}
