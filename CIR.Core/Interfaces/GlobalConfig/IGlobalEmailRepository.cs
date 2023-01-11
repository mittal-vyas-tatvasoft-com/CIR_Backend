using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.ViewModel.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalEmailRepository
    {
        Task<GlobalConfigurationEmailsGetModel> GetglobalEmailModelById(int id);
        string CreateOrUpdateGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailSaveModel);
    }
}
