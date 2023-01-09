using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.Common;

namespace CIR.Data.Data.Common
{
    public class CommonRepository : ICommonRepository
    {
        #region PROPERTIES

        private readonly CIRDbContext _CIRDBContext;

        #endregion

        #region CONSTRUCTORS
        public CommonRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion


        #region METHODS

        /// <summary>
        /// This method used by getcurrencies list
        /// </summary>
        /// <returns></returns>
        public List<Currency> GetCurrencies()
        {
            List<Currency> result = new List<Currency>();
            result = (from currency in _CIRDBContext.Currencies
                      select new Currency()
                      {
                          Id = currency.Id,
                          CodeName = currency.CodeName,
                          Symbol = currency.Symbol
                      }).ToList();
            return result;
        }

        /// <summary>
        /// This method used by getcountry list
        /// </summary>
        /// <returns></returns>
        public List<CountryCode> GetCountry()
        {
            List<CountryCode> result = new List<CountryCode>();
            result = (from country in _CIRDBContext.CountryCodes
                      select new CountryCode()
                      {
                          Id = country.Id,
                          CountryName = country.CountryName,
                          Code = country.Code
                      }).ToList();
            return result;
        }

        /// <summary>
        /// This method is used by getCultures
        /// </summary>
        /// <returns></returns>
        public List<Culture> GetCultures()
        {
            List<Culture> cultures = new List<Culture>();
            cultures = (from culture in _CIRDBContext.Cultures
                        select new Culture()
                        {
                            Id = culture.Id,
                            Name = culture.Name,
                            DisplayName = culture.DisplayName,
                        }).ToList();
            return cultures;
        }

        #endregion
    }
}
