using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.Interfaces.Utilities.SystemSettings;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Utilities.SystemSettings
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
