using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Common;
using CIR.Core.ViewModel.Utilities;
using Dapper;
using Microsoft.AspNetCore.Mvc;
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
				List<Currency> currenciesList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						currenciesList = connection.Query<Currency>("spGetCurrencies", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				if (currenciesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Currency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Currency>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = currenciesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
				List<CountryCode> countriesList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						countriesList = connection.Query<CountryCode>("spGetCountries", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (countriesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<CountryCode>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<CountryCode>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = countriesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });

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
				List<Culture> culturesList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						culturesList = connection.Query<Culture>("spGetCultures", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (culturesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<Culture>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = culturesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
				List<SubSite> subsitesList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						subsitesList = connection.Query<SubSite>("spGetSubSites", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (subsitesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<SubSite>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<SubSite>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = subsitesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
				List<RolePrivileges> rolePrivilegesList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						rolePrivilegesList = connection.Query<RolePrivileges>("spGetRolePrivileges", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (rolePrivilegesList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<RolePrivileges>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = rolePrivilegesList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method used by get Salutation type list
		/// <param name="code">
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetSalutationTypeList(string code)
		{
			try
			{
				List<UserLookupItemModel> salutationList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("Code", code);
						salutationList = connection.Query<UserLookupItemModel>("GetSalutationtypeList", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (salutationList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<UserLookupItemModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<UserLookupItemModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = salutationList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}


		/// <summary>
		/// This method used by get System codes list
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetSystemCodes()
		{
			try
			{
				List<SystemCodeModel> systemCodeList;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						systemCodeList = connection.Query<SystemCodeModel>("spGetSystemCodes", null, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				if (systemCodeList.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<SystemCodeModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}

				return new JsonResult(new CustomResponse<List<SystemCodeModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = systemCodeList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method used by check Is StringNullorEmpty
		/// </summary>
		/// <returns></returns>
		public Boolean IsStringNullorEmpty(string value)
		{
			if (value == null || value == string.Empty)
				return true;
			else
				return false;
		}


		#endregion
	}
}
