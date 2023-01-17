using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Website
{
    public class PortalService : IPortalService
    {
        private readonly IPortalRepository _portalRepository;
        public PortalService(IPortalRepository portalRepository)
        {
            _portalRepository = portalRepository;
        }

        public async Task<IActionResult> CreatePortal(PortalModel portalModel, long clientId)
        {
            return await _portalRepository.CreatePortal(portalModel, clientId);
        }

        public async Task<IActionResult> DisablePortal(long portalId)
        {
            return await _portalRepository.DisablePortal(portalId);
        }
    }
}
