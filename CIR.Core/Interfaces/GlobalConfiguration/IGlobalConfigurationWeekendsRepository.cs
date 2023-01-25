using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
    public interface IGlobalConfigurationWeekendsRepository
    {
        public Task<IActionResult> CreateGlobalConfigurationWeekends(GlobalConfigurationWeekends globalConfigurationWeekends);
        Task<Boolean> CountryWiseWeekendsExists(long countryId, long dayOfWeekId);
        Task<IActionResult> DeleteGlobalConfigurationWeekend(int id);
        Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true);
    }
}
