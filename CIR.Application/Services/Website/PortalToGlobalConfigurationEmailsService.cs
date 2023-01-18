using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Website
{
    public class PortalToGlobalConfigurationEmailsService : IPortalToGlobalConfigurationEmailsService
    {
        private readonly IPortalToGlobalConfigurationEmailsRepository _portalToGlobalConfigurationEmailsRepository;

        public PortalToGlobalConfigurationEmailsService(IPortalToGlobalConfigurationEmailsRepository portalToGlobalConfigurationEmailsRepository)
        {
            _portalToGlobalConfigurationEmailsRepository = portalToGlobalConfigurationEmailsRepository;
        }

        public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationEmails(List<PortalToGlobalConfigurationEmails> portalToGlobalConfigurationEmails)
        {
            return await _portalToGlobalConfigurationEmailsRepository.CreateOrUpdatePortalToGlobalConfigurationEmails(portalToGlobalConfigurationEmails);
        }

        public async Task<IActionResult> GetPortalToGlobalConfigurationEmailsList(int id)
        {
            return await _portalToGlobalConfigurationEmailsRepository.GetPortalToGlobalConfigurationEmailsList(id);
        }
    }
}
