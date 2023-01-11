using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;

namespace CIR.Core.Interfaces.Common
{
    public interface ICommonRepository
    {
        List<Currency> GetCurrencies();
        List<CountryCode> GetCountry();
        Task<List<Culture>> GetCultures();

        Task<List<SubSite>> GetSite();
        Task<List<RolePrivileges>> GetRolePrivileges();
    }
}
