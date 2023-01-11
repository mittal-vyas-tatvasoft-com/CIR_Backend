using CIR.Core.ViewModel.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalEmailService
    {
        string SaveGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailSaveModel);
        Task<GlobalConfigurationEmailsGetModel> globalEmailGetData(int id);
    }
}
