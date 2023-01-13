using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
    public interface IGlobalConfigurationEmailsService 
    {
        Task<IActionResult> CreateOrUpdateGlobalConfigurationEmails(List<GlobalConfigurationEmails> globalConfigurationEmails);
        Task<IActionResult> GetGlobalConfigurationEmailsDataList(int id);
    }
}
