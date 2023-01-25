using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationWeekendsService : IGlobalConfigurationWeekendsService
    {
        private readonly IGlobalConfigurationWeekendsRepository _globalConfigurationWeekendsRepository;

        public GlobalConfigurationWeekendsService(IGlobalConfigurationWeekendsRepository globalConfigurationWeekendsRepository)
        {
            _globalConfigurationWeekendsRepository = globalConfigurationWeekendsRepository;
        }

        public Task<IActionResult> CreateGlobalConfigurationWeekends(GlobalConfigurationWeekends globalConfigurationWeekends)
        {
            return _globalConfigurationWeekendsRepository.CreateGlobalConfigurationWeekends(globalConfigurationWeekends);
        }
        public async Task<bool> CountryWiseWeekendsExists(long countryId, long dayOfWeekId)
        {
            return await _globalConfigurationWeekendsRepository.CountryWiseWeekendsExists(countryId, dayOfWeekId);
        }

        public async Task<IActionResult> DeleteGlobalConfigurationWeekend(int id)
        {
            return await _globalConfigurationWeekendsRepository.DeleteGlobalConfigurationWeekend(id);
        }

        public async Task<IActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true)
        {
            return await _globalConfigurationWeekendsRepository.GetGlobalConfigurationWeekends(displayLength, displayStart, sortCol, filterCountryNameId, filterCountryCodeId, search, sortAscending);
        }
    }
}
