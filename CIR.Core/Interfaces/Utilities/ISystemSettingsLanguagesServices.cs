using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ISystemSettingsLanguagesServices
    {

        public Task<IActionResult> UpdateSystemSettingsLanguage(List<CulturesModel> culture);
    }
}
