using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalConfigurationWeekendsService : IGlobalConfigurationWeekendsService
    {
        private readonly IGlobalConfigurationWeekendsRepository _globalConfigurationWeekendsRepository;

        public GlobalConfigurationWeekendsService(IGlobalConfigurationWeekendsRepository globalConfigurationWeekendsRepository)
        {
            _globalConfigurationWeekendsRepository = globalConfigurationWeekendsRepository;
        }

        public Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends globalConfigurationWeekends)
        {
            return _globalConfigurationWeekendsRepository.CreateGlobalConfigurationWeekendsWeekends(globalConfigurationWeekends);
        }

        public async Task<IActionResult> DeleteGlobalConfigurationWeekend(int id)
        {
            return await _globalConfigurationWeekendsRepository.DeleteGlobalConfigurationWeekend(id);
        }

        public async Task<IActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
        {
            return await _globalConfigurationWeekendsRepository.GetGlobalConfigurationWeekends(displayLength, displayStart, sortCol, search, sortAscending);
        }
    }
}
