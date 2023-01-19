using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currenciesList = await _CIRDBContext.Currencies.Select(x => new Currency()
                {
                    Id = x.Id,
                    CodeName = x.CodeName,
                    Symbol = x.Symbol
                }).ToListAsync();

                if (currenciesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<Currency>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                return new JsonResult(new CustomResponse<List<Currency>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = currenciesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method used by getcountry list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countriesList = await _CIRDBContext.CountryCodes.ToListAsync();
                if (countriesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<CountryCode>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                return new JsonResult(new CustomResponse<List<CountryCode>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = countriesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method used by get culture list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCultures()
        {
            try
            {
                var culturesList = await _CIRDBContext.Cultures.ToListAsync();
                if (culturesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = culturesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method used by get site or section list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetSubSites()
        {
            try
            {
                var subsitesList = await _CIRDBContext.SubSites.ToListAsync();
                if (subsitesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<SubSite>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                return new JsonResult(new CustomResponse<List<SubSite>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = subsitesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method used by get role privileges list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetRolePrivileges()
        {
            try
            {
                var rolePrivilegesList = await _CIRDBContext.RolePrivileges.ToListAsync();
                if (rolePrivilegesList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = rolePrivilegesList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        #endregion
    }
}
