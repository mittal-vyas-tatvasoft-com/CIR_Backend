using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Website
{
	public class OfficeService : IOfficeService
	{
		private readonly IOfficesRepository _officesRepository;
		public OfficeService(IOfficesRepository officesRepository)
		{
			_officesRepository = officesRepository;
		}
		public async Task<IActionResult> CreateOrUpdateOffice(Offices offices)
		{
			return await _officesRepository.CreateOrUpdateOffice(offices);
		}
		public async Task<IActionResult> GetHolidays(int displayLength, int displayStart, string sortCol, string search, bool sortAscending = true)
		{
			return await _officesRepository.GetHolidays(displayLength, displayStart, sortCol, search, sortAscending);
		}
		public async Task<IActionResult> GetOfficesById(long id)
		{
			return await _officesRepository.GetOfficesById(id);
		}
		public async Task<IActionResult> DeleteOffice(long officeId)
		{
			return await _officesRepository.DeleteOffice(officeId);
		}
	}
}
