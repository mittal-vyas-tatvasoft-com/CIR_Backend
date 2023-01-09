using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;

namespace CIR.Core.Interfaces.Common
{
    public interface ICommonRepository
    {
        List<Currency> GetCurrencies();
        List<CountryCode> GetCountry();
        List<Culture> GetCultures();
    }
}
