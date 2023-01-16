using CIR.Core.Entities;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities.SystemSettings
{
    public interface ISystemSettingsLanguagesServices
    {
        
      public Task<IActionResult> UpdateSystemSettingsLanguage(CulturesModel culture);
    }
}
