using CIR.Core.Entities.Websites;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortal2GlobalConfigurationCutOffTimesRepository
    {
        Task<IActionResult> GetPortalToGlobalConfigurationCutOffTimesList(long PortalId);
        Task<IActionResult> UpdatePortalToGlobalConfigurationCutOffTimes(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes);
    }
}
