using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Utilities
{
    public interface ISytemSettingsLanguagesRepository
    {
        public Task<IActionResult> UpdateSystemSettingsLanguage(List<CulturesModel> culture);
    }
}
