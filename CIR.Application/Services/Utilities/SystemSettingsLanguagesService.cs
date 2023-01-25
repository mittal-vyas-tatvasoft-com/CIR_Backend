using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Utilities
{
    public class SystemSettingsLanguagesService : ISystemSettingsLanguagesServices
    {
        #region PROPERTIES
        private readonly ISytemSettingsLanguagesRepository _isystemSettingsLanguagesRepository;
        #endregion

        #region CONSTRUCTOR
        public SystemSettingsLanguagesService(ISytemSettingsLanguagesRepository systemSettingsLanguagesRepository)
        {
            _isystemSettingsLanguagesRepository = systemSettingsLanguagesRepository;
        }
        #endregion

        #region METHODS
        public Task<IActionResult> UpdateSystemSettingsLanguage(List<CulturesModel> culture)
        {
            return _isystemSettingsLanguagesRepository.UpdateSystemSettingsLanguage(culture);
        }
        #endregion
    }
}
