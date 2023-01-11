using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Common;

namespace CIR.Application.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public List<CountryCode> GetCountry()
        {
            return _commonRepository.GetCountry();
        }

        public List<Currency> GetCurrencies()
        {
            return _commonRepository.GetCurrencies();
        }
        public async Task<List<Culture>> GetCultures()
        {
            return await _commonRepository.GetCultures();
        }

        public async Task<List<SubSite>> GetSite()
        {
            return await _commonRepository.GetSite();
        }
        public async Task<List<RolePrivileges>> GetRolePrivileges()
        {
            return await _commonRepository.GetRolePrivileges();
        }
    }
}
