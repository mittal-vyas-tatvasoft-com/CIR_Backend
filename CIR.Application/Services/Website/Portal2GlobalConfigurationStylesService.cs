using CIR.Core.Entities;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Website
{
	public class Portal2GlobalConfigurationStylesService : IPortal2GlobalConfigurationStylesService
	{
		private readonly IPortal2GlobalConfigurationStylesRepository _portal2GlobalConfigurationStylesRepository;

		public Portal2GlobalConfigurationStylesService(IPortal2GlobalConfigurationStylesRepository portal2GlobalConfigurationStylesRepository)
		{
			_portal2GlobalConfigurationStylesRepository = portal2GlobalConfigurationStylesRepository;
		}

		public async Task<IActionResult> GetPortalToGlobalConfigurationStylesList(long portalId)
		{
			return await _portal2GlobalConfigurationStylesRepository.GetPortalToGlobalConfigurationStylesList(portalId);
		}
		public async Task<IActionResult> UpdatePortalToGlobalConfigurationStyles(List<Portal2GlobalConfigurationStyle> portal2GlobalConfigurationStyles)
		{
			return await _portal2GlobalConfigurationStylesRepository.UpdatePortalToGlobalConfigurationStyles(portal2GlobalConfigurationStyles);
		}
	}
}
