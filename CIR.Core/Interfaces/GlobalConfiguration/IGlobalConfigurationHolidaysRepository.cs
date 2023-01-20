using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationHolidaysRepository
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holiday);
		Task<IActionResult> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, string? countryCode, string? countryName, bool sortAscending = true);
		Task<IActionResult> GetHolidayById(long id);
		Task<IActionResult> UpdateHoliday(Holidays holidayModel);
		Task<IActionResult> DeleteHolidays(long holidayId);
	}
}
