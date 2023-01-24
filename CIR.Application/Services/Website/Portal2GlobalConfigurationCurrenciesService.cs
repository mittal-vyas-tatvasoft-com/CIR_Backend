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
    public class Portal2GlobalConfigurationCurrenciesService : IPortal2GlobalConfigurationCurrenciesService
    {
        private readonly IPortal2GlobalConfigurationCurrenciesRepository _portal2GlobalConfigurationCurrenciesRepository;
        public Portal2GlobalConfigurationCurrenciesService(IPortal2GlobalConfigurationCurrenciesRepository portal2GlobalConfigurationCurrenciesRepository)
        {
            _portal2GlobalConfigurationCurrenciesRepository = portal2GlobalConfigurationCurrenciesRepository;
        }

        public async Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(long PortalId)
        {
            return await _portal2GlobalConfigurationCurrenciesRepository.GetPortalToGlobalConfigurationCurrenciesList(PortalId);
        }
        public async Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrencies)
        {
            return await _portal2GlobalConfigurationCurrenciesRepository.UpdatePortalToGlobalConfigurationCurrencies(portal2GlobalConfigurationCurrencies);
        }
    }
}
