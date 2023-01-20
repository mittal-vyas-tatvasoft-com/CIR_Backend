using CIR.Core.Entities.Utilities;
using CIR.Core.Interfaces.Common;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Common
{
	public class CommonService : ICommonService
	{
		private readonly ICommonRepository _commonRepository;
		public CommonService(ICommonRepository commonRepository)
		{
			_commonRepository = commonRepository;
		}
		public async Task<IActionResult> GetCountries()
		{
			return await _commonRepository.GetCountries();
		}

		public async Task<IActionResult> GetCurrencies()
		{
			return await _commonRepository.GetCurrencies();
		}
		public async Task<IActionResult> GetCultures()
		{
			return await _commonRepository.GetCultures();
		}

		public async Task<IActionResult> GetSubSites()
		{
			return await _commonRepository.GetSubSites();
		}
		public async Task<IActionResult> GetRolePrivileges()
		{
			return await _commonRepository.GetRolePrivileges();
		}
		public async Task<IActionResult> GetSalutationTypeList(string code)
		{
			return await _commonRepository.GetSalutationTypeList(code);
		}
		public async Task<IActionResult> GetSystemCodes()
		{
			return await _commonRepository.GetSystemCodes();
		}
	}
}
