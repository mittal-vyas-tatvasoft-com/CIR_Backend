using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IGlobalConfigurationHolidaysRepository
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holiday);
	}
}
