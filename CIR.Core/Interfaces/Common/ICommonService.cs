using CIR.Core.Entities.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Common
{
	public interface ICommonService
	{
		Task<IActionResult> GetCurrencies();
		Task<IActionResult> GetCountries();
		Task<IActionResult> GetCultures();
		Task<IActionResult> GetSubSites();
		Task<IActionResult> GetRolePrivileges();
		Task<IActionResult> GetSalutationtypeList(string code);
		Task<IActionResult> GetSystemCodes();
	}
}
