﻿using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortalService
    {
        Task<IActionResult> CreateorUpdatePortal(PortalModel portalModel, long clientId);
        Task<IActionResult> DisablePortal(long portalId);

        Task<IActionResult> GetPortalsByClientId(int clientId);
    }
}
