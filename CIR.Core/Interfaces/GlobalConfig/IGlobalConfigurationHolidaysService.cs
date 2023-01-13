using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IGlobalConfigurationHolidaysService
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holidays);
	}
}
