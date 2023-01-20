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
    public class Portal2GlobalConfigurationCurrenciesService: IPortal2GlobalConfigurationCurrenciesService
    {
        private readonly IPortal2GlobalConfigurationCurrenciesRepository _portalToGlobalConfigurationCurrenciesRepository;
        public Portal2GlobalConfigurationCurrenciesService (IPortal2GlobalConfigurationCurrenciesRepository portalToGlobalConfigurationCurrenciesRepository)
        {
            _portalToGlobalConfigurationCurrenciesRepository = portalToGlobalConfigurationCurrenciesRepository;
        }

        public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(int id)
        {
            return await _portalToGlobalConfigurationCurrenciesRepository.GetPortalToGlobalConfigurationCurrenciesList(id);
        }
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portalToGlobalConfigurationCurrencies)
        {
            return await _portalToGlobalConfigurationCurrenciesRepository.UpdatePortalToGlobalConfigurationCurrencies(portalToGlobalConfigurationCurrencies);
        }
    }
}
