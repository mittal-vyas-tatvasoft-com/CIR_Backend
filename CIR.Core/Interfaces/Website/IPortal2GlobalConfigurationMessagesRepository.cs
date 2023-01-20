using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Website;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Website
{
	public interface IPortal2GlobalConfigurationMessagesRepository
	{
		Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(int portalId);
		Task<IActionResult> CreateOrUpdatePortalToGlobalConfigurationMessages(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessages);
	}
}
