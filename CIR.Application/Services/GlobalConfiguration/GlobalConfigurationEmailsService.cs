﻿using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationEmailsService : IGlobalConfigurationEmailsService
    {
        private readonly IGlobalConfigurationEmailsRepository _globalConfigurationEmailsRepository;

        public GlobalConfigurationEmailsService(IGlobalConfigurationEmailsRepository globalConfigurationEmailsRepository)
        {
            _globalConfigurationEmailsRepository = globalConfigurationEmailsRepository;
        }


        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationEmails(List<GlobalConfigurationEmails> globalConfigurationEmails)
        {
            return await _globalConfigurationEmailsRepository.CreateOrUpdateGlobalConfigurationEmails(globalConfigurationEmails);
        }

        public async Task<IActionResult> GetGlobalConfigurationEmailsDataList(int id)
        {
            return await _globalConfigurationEmailsRepository.GetGlobalConfigurationEmailsDataList(id);

        }
    }
}
