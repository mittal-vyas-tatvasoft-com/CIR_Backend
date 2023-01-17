using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortalService
    {
        Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId);
        Task<IActionResult> DisablePortal(long portalId);
    }
}
