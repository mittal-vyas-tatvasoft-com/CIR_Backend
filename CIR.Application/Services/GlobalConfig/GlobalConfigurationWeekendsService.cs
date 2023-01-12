using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalConfigurationWeekendsService :IGlobalConfigurationWeekendsService
    {
        private readonly IGlobalConfigurationWeekendsRepository _iweekendsRepository;

        public GlobalConfigurationWeekendsService(IGlobalConfigurationWeekendsRepository weekendsRepository)
        {
            _iweekendsRepository = weekendsRepository;
        }

        public Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends weekends)
        {
            return _iweekendsRepository.CreateGlobalConfigurationWeekendsWeekends(weekends);
        }

        public async Task<IActionResult> DeleteGlobalConfigurationWeekend(int id)
        {
            return await _iweekendsRepository.DeleteGlobalConfigurationWeekend(id);
        }

        public async Task<IActionResult> GetAllGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
        {
            return await _iweekendsRepository.GetAllGlobalConfigurationWeekends(displayLength, displayStart, sortCol, search, sortAscending);
        }
    }
}
