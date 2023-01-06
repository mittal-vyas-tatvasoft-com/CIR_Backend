using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        public  CommonService(ICommonRepository commonRepository)
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
    }
}
