using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalEmailsService
    {
        Task<IActionResult> CreateOrUpdateGlobalEmails(List<GlobalConfigurationEmailsModel> globalEmailsModel);
        Task<IActionResult> GetGlobalEmailsDataList(int id);
    }
}
