using CIR.Core.Entities.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationHolidaysService
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holidays);
		Task<IActionResult> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, string? countryCode, string? countryName, bool sortAscending = true);
		Task<IActionResult> GetHolidayById(long id);
		Task<IActionResult> UpdateHoliday(Holidays holidayModel);
		Task<IActionResult> DeleteHolidays(long holidayId);
	}
}
