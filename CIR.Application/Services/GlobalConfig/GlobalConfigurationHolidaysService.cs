using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
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
