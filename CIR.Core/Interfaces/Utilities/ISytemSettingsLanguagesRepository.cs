using CIR.Core.Entities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ISytemSettingsLanguagesRepository
    {
        public Task<IActionResult> UpdateSystemSettingsLanguage(List<CulturesModel> culture);
    }
}
