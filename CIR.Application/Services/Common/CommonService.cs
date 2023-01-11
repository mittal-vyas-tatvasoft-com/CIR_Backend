using CIR.Core.Interfaces.Common;
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
    }
}
