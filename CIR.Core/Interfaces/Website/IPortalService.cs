using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortalService
    {
        Task<IActionResult> DisablePortal(long portalId);

        Task<IActionResult> GetByClientId(int clientId);
        Task<IActionResult> GetById(int portalId);
        Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId);
        Task<IActionResult> UpdatePortal(PortalModel portalModel);
    }
}
