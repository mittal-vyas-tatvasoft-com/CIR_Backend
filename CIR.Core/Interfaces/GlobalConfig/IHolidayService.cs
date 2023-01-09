using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IHolidayService
	{
		Task<IActionResult> CreateOrUpdateHoliday(Holidays holidays);
	}
}
