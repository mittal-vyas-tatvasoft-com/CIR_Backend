using CIR.Core.Entities.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
    public interface IPortal2GlobalConfigurationCurrenciesService
    {
        Task<IActionResult> GetPortalToGlobalConfigurationCurrenciesList(long PortalId);
        Task<IActionResult> UpdatePortalToGlobalConfigurationCurrencies(List<Portal2GlobalConfigurationCurrency> portal2GlobalConfigurationCurrencies);
    }
}
