using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfiguration
{
	public class GlobalConfigurationHolidaysService : IGlobalConfigurationHolidaysService
	{
		private readonly IGlobalConfigurationHolidaysRepository _globalConfigurationHolidaysService;

		public GlobalConfigurationHolidaysService(IGlobalConfigurationHolidaysRepository globalConfigurationHolidaysService)
		{
			_globalConfigurationHolidaysService = globalConfigurationHolidaysService;
		}

		public Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holidays)
		{
			return _globalConfigurationHolidaysService.CreateOrUpdateGlobalConfigurationHolidays(holidays);
		}
	}
}
