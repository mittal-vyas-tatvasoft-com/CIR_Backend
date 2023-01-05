using CIR.Core.Entities.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Common
{
    public interface ICommonService
    {
        List<Currency> GetCurrencies();
        List<CountryCode> GetCountry();
    }
}
