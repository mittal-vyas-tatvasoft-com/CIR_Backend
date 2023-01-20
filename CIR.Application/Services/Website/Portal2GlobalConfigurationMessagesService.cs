using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Website
{
	public class Portal2GlobalConfigurationMessagesService : IPortal2GlobalConfigurationMessagesService
	{
		private readonly IPortal2GlobalConfigurationMessagesRepository _portalToGlobalConfigurationMessagesRepository;

		public Portal2GlobalConfigurationMessagesService(IPortal2GlobalConfigurationMessagesRepository portalToGlobalConfigurationMessagesRepository)
		{
			_portalToGlobalConfigurationMessagesRepository = portalToGlobalConfigurationMessagesRepository;
		}

		public async Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(int portalId)
		{
			return await _portalToGlobalConfigurationMessagesRepository.GetPortalToGlobalConfigurationMessagesList(portalId);
		}
		public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portalToGlobalConfigurationMessage)
		{
			return await _portalToGlobalConfigurationMessagesRepository.CreateOrUpdatePortalToGlobalConfigurationMessages(portalToGlobalConfigurationMessage);
		}
	}
}
