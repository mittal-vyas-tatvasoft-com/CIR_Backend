using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Utilities;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CIR.Data.Data.Utilities
{
	public class SystemSettingsLookupsRepository : ControllerBase, ILookupsRepository
	{
		#region PROPERTIES   
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public SystemSettingsLookupsRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
			   throw new ArgumentNullException(nameof(context));
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

				if (lookupItemsTextModel.Id == 0)
				{
					int displayOrder;
					var displayOrderId = _CIRDBContext.LookupItemsText.OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
					if (displayOrderId != null)
					{
						displayOrder = displayOrderId.DisplayOrder + 1;
					}
					else
					{
						displayOrder = 1;
					}

					LookupItem lookupItem = new()
					{
						SystemCodeId = lookupItemsTextModel.SystemCodeId,
					};
					_CIRDBContext.LookupItems.Add(lookupItem);
					_CIRDBContext.SaveChanges();

					LookupItemsText newLookupItem = new()
					{
						LookupItemId = lookupItem.LookupItemId,
						CultureId = lookupItemsTextModel.CultureId,
						DisplayOrder = displayOrder,
						Text = lookupItemsTextModel.Text,
						Active = lookupItemsTextModel.Active,
					};
					_CIRDBContext.LookupItemsText.Add(newLookupItem);
					_CIRDBContext.SaveChanges();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Lookup Item") });
				}
				else
				{
					var lookupItemTextData = _CIRDBContext.LookupItemsText.FirstOrDefault(x => x.Id == lookupItemsTextModel.Id);
					if (lookupItemTextData != null)
					{

						_CIRDBContext.LookupItemsText.Where(x => x.Id == lookupItemsTextModel.Id).ForEachAsync(item =>
						{
							item.Id = lookupItemsTextModel.Id;
							item.LookupItemId = lookupItemsTextModel.LookupItemId;
							item.CultureId = lookupItemsTextModel.CultureId;
							item.DisplayOrder = lookupItemTextData.DisplayOrder;
							item.Text = lookupItemsTextModel.Text;
							item.Active = lookupItemsTextModel.Active;
						});

						_CIRDBContext.SaveChanges();
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataUpdatedSuccessfully, "Lookup Item") });
					}
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgNotFound, "Lookup Item") });
				}
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

				if (code == string.Empty || code == null)
				{
					code = string.Empty;
				}

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
				List<LookupItemsText> lookupsItemList = new List<LookupItemsText>();
				var dictionaryobj = new Dictionary<string, object>
			{
					{"CultureId", cultureId},
					{ "Code", code},
					{ "SearchLookupItems", searchLookupItems}
			};

				DataTable lookupItemsDatatable = SQLHelper.ExecuteSqlQueryWithParams("spGetLookupItemList", dictionaryobj);
				if (lookupItemsDatatable != null)
				{
					foreach (DataRow row in lookupItemsDatatable.Rows)
					{
						LookupItemsText lookupModel = new LookupItemsText
						{
							Id = Convert.ToInt64(row["Id"]),
							Text = Convert.ToString(row["Text"]),
							LookupItemId = Convert.ToInt64(row["LookupItemId"]),
							Active = Convert.ToBoolean(row["Active"]),
							CultureId = Convert.ToInt64(row["CultureId"])
						};
						lookupsItemList.Add(lookupModel);
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
			List<CulturalCodesModel> codeList = new List<CulturalCodesModel>();

			var dictionaryobj = new Dictionary<string, object>
			{
					{"CultureId", cultureId},
					{ "Code", code},
					{ "SearchCultureCode", searchCultureCode},
			 };

			DataTable cultureCodeDatatable = SQLHelper.ExecuteSqlQueryWithParams("spGetCultureCodeList", dictionaryobj);
			if (cultureCodeDatatable != null)
			{
				foreach (DataRow row in cultureCodeDatatable.Rows)
				{
					CulturalCodesModel culturalCodesModel = new CulturalCodesModel
					{
						CultureId = Convert.ToInt64(row["CultureId"]),
						SystemCodeId = Convert.ToInt64(row["SystemCodeId"]),
						Code = Convert.ToString(row["Code"]),
						CultureDisplayText = Convert.ToString(row["DisplayName"])
					};
					codeList.Add(culturalCodesModel);
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
			var checkItemExist = await _CIRDBContext.LookupItemsText.Where(x => x.CultureId == cultureId && x.LookupItemId == lookupItemId).FirstOrDefaultAsync();

			if (checkItemExist != null)
			{
				return true;
			}
			return false;
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
				var lookupItem = await (from lookUp in _CIRDBContext.LookupItems
										join code in _CIRDBContext.SystemCodes
										on lookUp.SystemCodeId equals code.Id
										join text in _CIRDBContext.LookupItemsText
										on lookUp.LookupItemId equals text.LookupItemId
										select new LookupItemsTextModel()
										{
											Id = text.Id,
											SystemCodeId = code.Id,
											LookupItemId = text.LookupItemId,
											CultureId = text.CultureId,
											DisplayOrderId = text.DisplayOrder,
											Text = text.Text,
											Active = text.Active,
											Code = code.Code
										}).Where(x => x.Id == id).FirstOrDefaultAsync();

				if (lookupItem == null)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgNotFound, "Lookup Item") });
				}
				return new JsonResult(new CustomResponse<LookupItemsTextModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = lookupItem });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
