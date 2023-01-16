using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationHolidaysService
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holidays);
	}
}
