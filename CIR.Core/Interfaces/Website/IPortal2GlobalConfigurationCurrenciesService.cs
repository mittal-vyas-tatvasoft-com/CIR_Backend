using CIR.Core.Entities.Websites;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortal2GlobalConfigurationCurrenciesService
    {
        Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(int id);
        Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portalToGlobalConfigurationCurrencies);
    }
}
