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
		private readonly IPortal2GlobalConfigurationMessagesRepository _portal2GlobalConfigurationMessagesRepository;

		public Portal2GlobalConfigurationMessagesService(IPortal2GlobalConfigurationMessagesRepository portal2GlobalConfigurationMessagesRepository)
		{
			_portal2GlobalConfigurationMessagesRepository = portal2GlobalConfigurationMessagesRepository;
		}

		public async Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(long portalId)
		{
			return await _portal2GlobalConfigurationMessagesRepository.GetPortalToGlobalConfigurationMessagesList(portalId);
		}
		public async Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessage)
		{
			return await _portal2GlobalConfigurationMessagesRepository.CreateOrUpdatePortalToGlobalConfigurationMessages(portal2GlobalConfigurationMessage);
		}
	}
}
