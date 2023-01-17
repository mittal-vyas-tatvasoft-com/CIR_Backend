using CIR.Core.Entities.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortalToGlobalConfigurationEmailsService
    {
        Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationEmails(List<PortalToGlobalConfigurationEmails> portalToGlobalConfigurationEmails);
        Task<IActionResult> GetPortalToGlobalConfigurationEmailsList(int id);
    }
}
