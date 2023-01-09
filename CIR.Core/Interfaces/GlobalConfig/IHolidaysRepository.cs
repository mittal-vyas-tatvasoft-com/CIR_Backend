using CIR.Core.Entities.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfig
{
	public interface IHolidaysRepository
	{
		Task<IActionResult> CreateOrUpdateHoliday(Holidays holiday);
	}
}
