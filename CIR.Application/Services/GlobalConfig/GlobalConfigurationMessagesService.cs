using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
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
