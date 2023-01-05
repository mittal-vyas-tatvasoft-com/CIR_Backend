using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.GlobalConfig
{
    public class GlobalCurrencyService : IGlobalCurrencyService
    {
        private readonly IGlobalCurrencyRepository _currencyRepository;
        public GlobalCurrencyService(IGlobalCurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public List<GlobalConfigurationCurrencyModel> GetCurrencyCountryWise(int countryId)
        {
            return _currencyRepository.GetCurrencyCountryWise(countryId);
        }

        public string CreateOrUpdateGlobalCurrencies(List<GlobalCurrencyModel> globalCurrencyModel)
        {
            return _currencyRepository.CreateOrUpdateGlobalCurrencies(globalCurrencyModel);
        }
    }
}
