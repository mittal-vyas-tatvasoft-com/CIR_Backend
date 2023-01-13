using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalConfigEmailService : IGlobalEmailService
    {
        private readonly IGlobalEmailRepository _saveEmailRepository;

        public GlobalConfigEmailService(IGlobalEmailRepository saveEmailRepository)
        {
            _saveEmailRepository = saveEmailRepository;
        }

        public async Task<IActionResult> SaveGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailSaveModel)
        {
            return await _saveEmailRepository.CreateOrUpdateGlobalEmail(globalEmailSaveModel);
        }

        public async Task<IActionResult> globalEmailGetData(int id)
        {
            return await _saveEmailRepository.GetglobalEmailModelById(id);

        }
    }
}
