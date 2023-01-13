using CIR.Core.Entities.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationWeekendsService
    {
        public Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends weekends);
        public Task<IActionResult> DeleteGlobalConfigurationWeekend(int id);
        public Task<IActionResult> GetAllGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending);
    }
}
