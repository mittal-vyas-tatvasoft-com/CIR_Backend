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
    public class GlobalEmailsService : IGlobalEmailsService
    {
        private readonly IGlobalEmailsRepository _globalEmailsRepository;

        public GlobalEmailsService(IGlobalEmailsRepository globalEmailsRepository)
        {
            _globalEmailsRepository = globalEmailsRepository;
        }

        public async Task<IActionResult> CreateOrUpdateGlobalEmails(List<GlobalConfigurationEmailsModel> globalEmailsModel)
        {
            return await _globalEmailsRepository.CreateOrUpdateGlobalEmails(globalEmailsModel);
        }

        public async Task<IActionResult> GetGlobalEmailsDataList(int id)
        {
            return await _globalEmailsRepository.GetGlobalEmailsDataList(id);

        }
    }
}
