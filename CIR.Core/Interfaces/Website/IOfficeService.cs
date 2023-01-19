using CIR.Core.Entities.Websites;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Website
{
	public interface IOfficeService
	{
		Task<IActionResult> CreateOrUpdateOffice(Offices offices);
		Task<IActionResult> GetHolidays(int displayLength, int displayStart, string sortCol, string search, bool sortAscending = true);
		Task<IActionResult> GetOfficesById(long id);
		Task<IActionResult> DeleteOffice(long officeId);

	}
}
