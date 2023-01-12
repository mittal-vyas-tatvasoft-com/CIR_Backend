using CIR.Core.Entities.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalConfigurationWeekendsRepository
    {
         Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends weekends);
         Task<IActionResult> DeleteGlobalConfigurationWeekend(int id);
         Task<ActionResult> GetAllGlobalConfigurationWeekends(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true);
    }
}
