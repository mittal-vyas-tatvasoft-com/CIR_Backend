using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Website
{
    public class PortalToGlobalConfigurationCurrenciesService: IPortalToGlobalConfigurationCurrenciesService
    {
        private readonly IPortalToGlobalConfigurationCurrenciesRepository _portalToGlobalConfigurationCurrenciesRepository;
        public PortalToGlobalConfigurationCurrenciesService (IPortalToGlobalConfigurationCurrenciesRepository portalToGlobalConfigurationCurrenciesRepository)
        {
            _portalToGlobalConfigurationCurrenciesRepository = portalToGlobalConfigurationCurrenciesRepository;
        }

        public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(int id)
        {
            return await _portalToGlobalConfigurationCurrenciesRepository.GetPortalToGlobalConfigurationCurrenciesList(id);
        }
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<PortalToGlobalConfigurationCurrency> portalToGlobalConfigurationCurrencies)
        {
            return await _portalToGlobalConfigurationCurrenciesRepository.UpdatePortalToGlobalConfigurationCurrencies(portalToGlobalConfigurationCurrencies);
        }
    }
}
