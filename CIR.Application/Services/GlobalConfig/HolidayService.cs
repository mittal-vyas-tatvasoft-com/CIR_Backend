using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.GlobalConfig
{
	public class HolidayService : IHolidayService
	{
		private readonly IHolidaysRepository _holidaysRepository;

		public HolidayService(IHolidaysRepository holidaysRepository)
		{
			_holidaysRepository = holidaysRepository;
		}

		public Task<IActionResult> CreateOrUpdateHoliday(Holidays holidays)
		{
			return _holidaysRepository.CreateOrUpdateHoliday(holidays);
		}
	}
}
