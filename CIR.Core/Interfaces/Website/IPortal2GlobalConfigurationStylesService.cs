using CIR.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
	public interface IPortal2GlobalConfigurationStylesService
	{
		Task<IActionResult> GetPortalToGlobalConfigurationStylesList(long portalId);
		Task<IActionResult> UpdatePortalToGlobalConfigurationStyles(List<Portal2GlobalConfigurationStyle> portal2GlobalConfigurationStyles);
	}
}
