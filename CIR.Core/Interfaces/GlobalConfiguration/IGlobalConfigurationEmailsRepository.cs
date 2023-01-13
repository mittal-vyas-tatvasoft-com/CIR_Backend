using CIR.Core.Entities.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
    public interface IGlobalConfigurationEmailsRepository
    {
        Task<IActionResult> CreateOrUpdateGlobalConfigurationEmails(List<GlobalConfigurationEmails> globalConfigurationEmails);
        Task<IActionResult> GetGlobalConfigurationEmailsDataList(int id);
    }
}
