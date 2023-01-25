using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortal2GlobalConfigurationCutOffTimesService
    {
        Task<IActionResult> GetPortalToGlobalConfigurationCutOffTimesList(long PortalId);
        Task<IActionResult> UpdatePortalToGlobalConfigurationCutOffTimes(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes);
    }
}
