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
		public async Task<IActionResult> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, string countrycode, string countryname, bool sortAscending = true)
		{
			return await _globalConfigurationHolidaysService.GetGlobalConfigurationHolidays(displayLength, displayStart, sortCol, search, countrycode, countryname, sortAscending);
		}
		public async Task<IActionResult> GetHolidayById(long id)
		{
			return await _globalConfigurationHolidaysService.GetHolidayById(id);
		}
		public async Task<IActionResult> UpdateHoliday(Holidays HolidayModel)
		{
			return await _globalConfigurationHolidaysService.UpdateHoliday(HolidayModel);
		}
		public async Task<IActionResult> DeleteHolidays(long holidayId)
		{
			return await _globalConfigurationHolidaysService.DeleteHolidays(holidayId);
		}
	}
}
