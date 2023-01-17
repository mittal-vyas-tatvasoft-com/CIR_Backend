using CIR.Common.CommonModels;
using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Users;
using CIR.Core.Entities.Utilities;
using CIR.Core.Interfaces.Common;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


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
				return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = rolePrivilegesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method used by get Salutation type list
		/// <param name="code">
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetSalutationtypeList(string code)
		{
			List<LookupItemsText> lookupItemList = new List<LookupItemsText>();

			try
			{
				return await GetSalutationList(code);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		public async Task<IActionResult> GetSalutationList(string code)
		{
			List<LookupItemsText> SalutationList = new List<LookupItemsText>();
			var dictionaryobj = new Dictionary<string, object>
			{
				{ "Code", code}
			};

			var dt = SQLHelper.ExecuteSqlQueryWithParams("spGetSalutationTypeList", dictionaryobj);
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					LookupItemsText lookupItemsText = new LookupItemsText();

					lookupItemsText.Id = Convert.ToInt64(row["Id"]);
					lookupItemsText.Text = Convert.ToString(row["Text"]);
					SalutationList.Add(lookupItemsText);
				}
			}
			return new JsonResult(new CustomResponse<List<LookupItemsText>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = SalutationList });
		}
		#endregion
	}
}
