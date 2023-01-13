
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

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
