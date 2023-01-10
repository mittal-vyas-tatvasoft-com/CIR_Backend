using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

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
        /// This method used by get culture list
        /// </summary>
        /// <returns></returns>
        public async Task<List<Culture>> GetCultures()
        {
            var cultureList = await _CIRDBContext.Cultures.ToListAsync();
            return cultureList;
        }

        /// <summary>
        /// This method used by get site or section list
        /// </summary>
        /// <returns></returns>
        public async Task<List<SubSite>> GetSite()
        {
            var siteList = await _CIRDBContext.SubSites.ToListAsync();
            return siteList;
        }

        /// <summary>
        /// This method used by get role privileges list
        /// </summary>
        /// <returns></returns>
        public async Task<List<RolePrivileges>> GetRolePrivileges()
        {
            var result = await _CIRDBContext.RolePrivileges.ToListAsync();
            return result;
        }

        #endregion
    }
}
