using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationMessagesService : IGlobalConfigurationMessagesService
    {
        private readonly IGlobalConfigurationMessagesRepository _globalConfigurationMessagesRepository;

        public GlobalConfigurationMessagesService(IGlobalConfigurationMessagesRepository globalConfigurationMessagesRepository)
        {
            _globalConfigurationMessagesRepository = globalConfigurationMessagesRepository;
        }

        public async Task<IActionResult> GetGlobalConfigurationMessagesList(int cultureId)
        {
            return await _globalConfigurationMessagesRepository.GetGlobalConfigurationMessagesList(cultureId);
        }
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationMessages(List<GlobalConfigurationMessages> globalConfigurationMessages)
        {
            return await _globalConfigurationMessagesRepository.CreateOrUpdateGlobalConfigurationMessages(globalConfigurationMessages);
        }
    }
}
