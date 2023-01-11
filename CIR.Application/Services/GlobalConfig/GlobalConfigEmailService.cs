using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.GlobalConfig;
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

        //public async Task<GlobalConfigurationEmails> globalEmailGetData(int id)
        //{
        //    var user = await _saveEmailRepository.GetglobalEmailModelById(id);
        //    return user;
        //}

        public string SaveGlobalEmail(List<GlobalConfigurationEmailsModel> globalEmailSaveModel)
        {
            return _saveEmailRepository.CreateOrUpdateGlobalEmail(globalEmailSaveModel);
        }

       //public async Task<GlobalConfigurationEmails> globalEmailGetData(int id)
       // {
       //     var user = await _saveEmailRepository.GetglobalEmailModelById(id);
       //     return user;
       // }

        public async Task<GlobalConfigurationEmailsGetModel> globalEmailGetData(int id)
        {
            var user = await _saveEmailRepository.GetglobalEmailModelById(id);
            return user;
        }
    }
}
