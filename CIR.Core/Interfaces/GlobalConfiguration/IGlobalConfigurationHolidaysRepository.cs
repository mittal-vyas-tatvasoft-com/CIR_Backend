using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.GlobalConfiguration
{
	public interface IGlobalConfigurationHolidaysRepository
	{
		Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holiday);
		Task<HolidayModel> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true);
		Task<IActionResult> GetHolidayById(long id);
		Task<IActionResult> UpdateHoliday(Holidays HolidayModel);
		Task<IActionResult> DeleteHolidays(long holidayId);
	}
}
