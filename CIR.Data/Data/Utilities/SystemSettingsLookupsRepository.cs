using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Utilities;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using CIR.Data.Data.Common;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CIR.Data.Data.Utilities
{
	public class SystemSettingsLookupsRepository : ControllerBase, ILookupsRepository
	{
		#region PROPERTIES   
		private readonly CIRDbContext _CIRDBContext;
		private readonly CommonRepository _commonRepository;
		#endregion

		#region CONSTRUCTOR
		public SystemSettingsLookupsRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
			   throw new ArgumentNullException(nameof(context));

			_commonRepository = new CommonRepository(context);
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method is used by create and update methods of Lookups
		/// </summary>
		/// <param name="lookupItemsTextModel></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextModel)
		{
			try
			{
				if (lookupItemsTextModel.Code == null || lookupItemsTextModel.CultureId == 0)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
				}
				var result = 0;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@Id", lookupItemsTextModel.Id);
						parameters.Add("@SystemCodeId", lookupItemsTextModel.SystemCodeId);
						parameters.Add("@LookupItemId", lookupItemsTextModel.LookupItemId);
						parameters.Add("@CultureId", lookupItemsTextModel.CultureId);
						parameters.Add("@Text", lookupItemsTextModel.Text);
						parameters.Add("@Active", lookupItemsTextModel.Active);
						result = connection.Execute("spCreateOrUpdateLookupItem", parameters, commandType: CommandType.StoredProcedure);

					}
				}
				if (result != 0)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Lookup Item") });
				}
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered LookupItems list using LINQ
		/// </summary>
		/// <param name="cultureId"> </param>
		/// <param name="code">cultureCode to get LookupItem</param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="searchCultureCode"> word that we want to search in CultureCodeList </param>
		/// <param name="sortAscending">it will indicate sorting direction</param>
		/// <returns> filtered list of LookupItems </returns>
		public async Task<IActionResult> GetAllCultureCodeList(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true)
		{
			long? cultureCodeCultureId = 0;
			LookupsModel lookupModel = new LookupsModel();

			try
			{
				searchCultureCode ??= string.Empty;

				if (cultureId == null)
				{
					cultureCodeCultureId = 0;
				}
				else
				{
					cultureCodeCultureId = cultureId;
				}

				if (_commonRepository.IsStringNullorEmpty(code) == true)
					code = string.Empty;

				lookupModel.CulturalCodesList = GetCulturalCodesList(cultureCodeCultureId, code, searchCultureCode);

				if (lookupModel == null)
				{
					return new JsonResult(new CustomResponse<LookupsModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<LookupsModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = lookupModel });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });

			}
		}

		/// <summary>
		/// This method retuns filtered LookupItems list
		/// </summary>
		/// <param name="code">Defalut Loaded Code=Salutation-type, It will change base on dropdown selection change</param>
		/// <param name="cultureId"> Default CultureId = 1 , It will change base on Culture dropdown change</param>
		/// <param name="searchLookupItems"> filter LookupItemList</param>
		/// <param name="sortAscending"> filter LookupItemList</param>
		/// <returns> filtered list of LookupItemsList </returns>
		public async Task<IActionResult> GetAllLookupsItems(long cultureId, string code, string? searchLookupItems, bool sortAscending = true)
		{
			try
			{
				List<LookupItemsText> lookupsItemList = new();
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@CultureId", cultureId);
						parameters.Add("@Code", code);
						parameters.Add("@SearchLookupItems", searchLookupItems);
						lookupsItemList = connection.Query<LookupItemsText>("spGetLookupItemList", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}

				if (lookupsItemList.Count == 0)
				{
					return new JsonResult(new CustomResponse<LookupItemsText>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}

				return new JsonResult(new CustomResponse<List<LookupItemsText>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = lookupsItemList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered LookupItems list
		/// </summary>
		/// <param name="code">Defalut Loaded Code=null, It will change base on dropdown selection change</param>
		/// <param name="cultureId"> Default CultureId = 0 , It will change base on Culture dropdown change</param>
		/// <param name="searchCultureCode"> filter CultureCodeList</param>
		/// <returns> filtered list of LookupItemsList </returns>
		private List<CulturalCodesModel> GetCulturalCodesList(long? cultureId, string code, string searchCultureCode = null)
		{
			List<CulturalCodesModel> codeList = new();

			using (DbConnection dbConnection = new DbConnection())
			{
				using (var connection = dbConnection.Connection)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("@CultureId", cultureId);
					parameters.Add("@Code", code);
					parameters.Add("@SearchCultureCode", searchCultureCode);
					codeList = connection.Query<CulturalCodesModel>("spGetCultureCodeList", parameters, commandType: CommandType.StoredProcedure).ToList();
				}
			}

			return codeList;
		}

		/// <summary>
		/// this meethod checks if LookupItem exists or not based on input cultureId,LookupItemId
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="lookupItemId"></param>
		/// <returns> if LookupItem exists true else false </returns>
		public async Task<Boolean> LookupItemExists(long cultureId, long lookupItemId)
		{
			using (DbConnection dbConnection = new DbConnection())
			{
				using (var connection = dbConnection.Connection)
				{
					DynamicParameters parameters = new DynamicParameters();
					parameters.Add("@CultureId", cultureId);
					parameters.Add("@LookupItemId", lookupItemId);
					var checkItemExist = connection.Query("spLookupItemExists", parameters, commandType: CommandType.StoredProcedure);
					if (checkItemExist.FirstOrDefault() != null)
					{
						return true;
					}
					return false;
				}
			}
		}

		/// <summary>
		/// This method will return look up items of given id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetLookupById(int id)
		{
			try
			{
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("@id", id);
						List<LookupItemsTextModel> lookupItem = connection.Query<LookupItemsTextModel>("spGetLookupById", parameters, commandType: CommandType.StoredProcedure).ToList();
						if (lookupItem.Count == 0)
						{
							return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgNotFound, "Lookup Item") });
						}
						return new JsonResult(new CustomResponse<List<LookupItemsTextModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = lookupItem });
					}
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
