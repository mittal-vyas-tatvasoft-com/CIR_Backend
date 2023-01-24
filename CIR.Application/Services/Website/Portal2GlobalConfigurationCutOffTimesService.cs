using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using CIR.Core.ViewModel.Website;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Website
{
    public class Portal2GlobalConfigurationCutOffTimesService : IPortal2GlobalConfigurationCutOffTimesService
    {
        private readonly IPortal2GlobalConfigurationCutOffTimesRepository _portal2GlobalConfigurationCutOffTimesRepository;
        public Portal2GlobalConfigurationCutOffTimesService(IPortal2GlobalConfigurationCutOffTimesRepository portal2GlobalConfigurationCutOffTimesRepository)
        {
            _portal2GlobalConfigurationCutOffTimesRepository = portal2GlobalConfigurationCutOffTimesRepository;
        }

        public async Task<IActionResult> GetPortalToGlobalConfigurationCutOffTimesList(long PortalId)
        {
            return await _portal2GlobalConfigurationCutOffTimesRepository.GetPortalToGlobalConfigurationCutOffTimesList(PortalId);
        }
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCutOffTimes(List<Portal2GlobalConfigurationCutOffTimesModel> portal2GlobalConfigurationCutOffTimes)
        {
            return await _portal2GlobalConfigurationCutOffTimesRepository.UpdatePortalToGlobalConfigurationCutOffTimes(portal2GlobalConfigurationCutOffTimes);
        }
    }
}
