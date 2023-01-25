using CIR.Core.Entities.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortal2GlobalConfigurationMessagesService
    {
        Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(long portalId);
        Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessages);
    }
}
