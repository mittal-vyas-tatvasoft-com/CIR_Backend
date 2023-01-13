﻿using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalEmailRepository
    {
        Task<IActionResult> GetglobalEmailModelById(int id);
        Task<IActionResult> CreateOrUpdateGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailSaveModel);
    }
}
