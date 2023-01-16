using CIR.Application.Services.Users;
using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.Interfaces.Utilities.SystemSettings;
using CIR.Core.ViewModel;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using CIR.Core.ViewModel.Utilities.SystemSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CIR.Common.Helper;
using System.Data;
using CIR.Core.Entities.Utilities.SystemSettings;
using CIR.Core.Entities;

namespace CIR.Data.Data.Utilities.SystemSettings
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
		/// <param name="CultureID"></param>
		/// <param name="Code"></param>
		/// <param name="LookupItemID"></param>
		/// <returns>Success status if its valid else failure</returns>
		public async Task<IActionResult> CreateOrUpdateLookupItem(LookupsModel lookupsModel)
		{
			try
			{
				if (lookupsModel.Code == null || lookupsModel.CultureId == 0)
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
				}

				int displayOrder;
				var displayOrderId = _CIRDBContext.LookupItemsText.OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
				if (displayOrderId != null)
					displayOrder = displayOrderId.DisplayOrder + 1;
				else
					displayOrder = 0;

				LookupItemsText lookupsData = new()
				{
					CultureId = lookupsModel.CultureId,
					Id = lookupsModel.Id,
					LookupItemId = lookupsModel.LookupItemId,
					DisplayOrder = displayOrder,
					Text = lookupsModel.Text,
					Active = lookupsModel.Active,
				};

				if (lookupsModel.Id > 0)
					_CIRDBContext.LookupItemsText.Update(lookupsData);
				else
					_CIRDBContext.LookupItemsText.Add(lookupsData);

				_CIRDBContext.SaveChanges();
				return new JsonResult(new CustomResponse<LookupItemsText>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = lookupsData });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered LookupItems list using LINQ
		/// </summary>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of LookupItems </returns>
		public async Task<LookupsModel> GetAllLookupsItems(string sortCol, string? searchCultureCode, string? searchLookupItems, bool sortAscending = true)
		{
			LookupsModel lookupModel = new LookupsModel();
			try
			{
				searchCultureCode ??= string.Empty;
				searchLookupItems ??= string.Empty;

				lookupModel.SystemCodeList = (from sc in _CIRDBContext.SystemCodes
											  select new SystemCode()
											  {
												  Id = sc.Id,
												  Code = sc.Code
											  }).ToList();

				lookupModel.CulturalCodesList = GetCulturalCodesList(0, string.Empty, searchCultureCode);
				lookupModel.LookupItemsList = GetLookupItemList(1, "Salutation-type", searchLookupItems);

				return lookupModel;
			}
			catch (Exception ex)
			{
				return lookupModel;
			}
		}

		/// <summary>
		/// This method retuns filtered LookupItems list
		/// </summary>
		/// <param name="Code">Defalut Loaded Code=Salutation-type, It will change base on dropdown selection change</param>
		/// <param name="CultureId"> Default CultureId = 1 , It will change base on Culture dropdown change</param>
		/// <returns> filtered list of LookupItemsList </returns>
		private List<LookupItemsText> GetLookupItemList(Int64 cultureId, string code, string searchLookupItems = null)
		{
			List<LookupItemsText> lookupsItemList = new List<LookupItemsText>();
			var dictionaryobj = new Dictionary<string, object>
			{
					{"CultureId", cultureId},
					{ "Code", code},
					{ "SearchLookupItems", searchLookupItems},
			};

			DataTable dt = SQLHelper.ExecuteSqlQueryWithParams("spGetLookupItemList", dictionaryobj);
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					LookupItemsText lookupModel = new LookupItemsText();

					lookupModel.Id = Convert.ToInt64(row["Id"]);
					lookupModel.Text = Convert.ToString(row["Text"]);
					lookupModel.LookupItemId = Convert.ToInt64(row["LookupItemId"]);
					lookupModel.Active = Convert.ToBoolean(row["Active"]);
					lookupModel.CultureId = Convert.ToInt64(row["CultureId"]);
					lookupsItemList.Add(lookupModel);
				}
			}

			return lookupsItemList;
		}

		/// <summary>
		/// This method retuns filtered LookupItems list
		/// </summary>
		/// <param name="Code">Defalut Loaded Code=null, It will change base on dropdown selection change</param>
		/// <param name="CultureId"> Default CultureId = 0 , It will change base on Culture dropdown change</param>
		/// <returns> filtered list of LookupItemsList </returns>
		private List<CulturalCodesModel> GetCulturalCodesList(Int64 cultureId, string code, string searchCultureCode = null)
		{
			List<CulturalCodesModel> codeList = new List<CulturalCodesModel>();

			var dictionaryobj = new Dictionary<string, object>
			{
					{"CultureId", cultureId},
					{ "Code", code},
					{ "SearchCultureCode", searchCultureCode},
			 };

			DataTable dt = SQLHelper.ExecuteSqlQueryWithParams("spGetCultureCodeList", dictionaryobj);
			if (dt != null)
			{
				foreach (DataRow row in dt.Rows)
				{
					CulturalCodesModel culturalCodesModel = new CulturalCodesModel();
					culturalCodesModel.CultureId = Convert.ToInt64(row["CultureId"]);
					culturalCodesModel.SystemCodeId = Convert.ToInt64(row["SystemCodeId"]);
					culturalCodesModel.Code = Convert.ToString(row["Code"]);
					culturalCodesModel.CultureDisplayText = Convert.ToString(row["DisplayName"].ToString());
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
			var checkItemExist = await _CIRDBContext.LookupItemsText.Where(x => x.CultureId == cultureId && x.LookupItemId != lookupItemId).FirstOrDefaultAsync();

			if (checkItemExist != null)
			{
				return true;
			}
			return false;
		}
		#endregion
	}
}
