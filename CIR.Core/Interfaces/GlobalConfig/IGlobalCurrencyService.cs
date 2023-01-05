using CIR.Core.Entities.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.GlobalConfig
{
    public interface IGlobalCurrencyService
    {
        List<GlobalConfigurationCurrencyModel> GetCurrencyCountryWise(int countryId);
        string CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel);
    }
}
