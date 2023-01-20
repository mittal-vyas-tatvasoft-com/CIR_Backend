using CIR.Common.CustomResponse;
using CIR.Common.Data;
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
        /// <param name="lookupItemsTextmodel></param>
        /// <returns>Success status if its valid else failure</returns>
        public async Task<IActionResult> CreateOrUpdateLookupItem(LookupItemsTextModel lookupItemsTextmodel)
        {
            try
            {
                if (lookupItemsTextmodel.Code == null || lookupItemsTextmodel.CultureId == 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
                }

                if (lookupItemsTextmodel.Id == 0)
                {
                    int displayOrder;
                    var displayOrderId = _CIRDBContext.LookupItemsText.OrderByDescending(x => x.DisplayOrder).FirstOrDefault();
                    if (displayOrderId != null)
                        displayOrder = displayOrderId.DisplayOrder + 1;
                    else
                        displayOrder = 1;

                    var lookupItemId = (from sc in _CIRDBContext.SystemCodes
                                        join lookup in _CIRDBContext.LookupItems on sc.Id equals lookup.SystemCodeId
                                        select new LookupItem
                                        {
                                            LookupItemId = lookup.LookupItemId
                                        }).FirstOrDefault();

                    LookupItemsText newLookupItem = new()
                    {
                        LookupItemId = lookupItemsTextmodel.LookupItemId,
                        CultureId = lookupItemsTextmodel.CultureId,
                        DisplayOrder = displayOrder,
                        Text = lookupItemsTextmodel.Text,
                        Active = lookupItemsTextmodel.Active,
                    };
                    _CIRDBContext.LookupItemsText.Add(newLookupItem);
                    _CIRDBContext.SaveChanges();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Lookup Item Saved Successfully." });
                }
                else
                {
                    var lookupItemTextData = _CIRDBContext.LookupItemsText.FirstOrDefault(x => x.Id == lookupItemsTextmodel.Id);
                    if (lookupItemTextData != null)
                    {
                        LookupItemsText updateItem = lookupItemTextData;
                        updateItem.Text = lookupItemsTextmodel.Text;
                        updateItem.Active = lookupItemsTextmodel.Active;

                        _CIRDBContext.LookupItemsText.Update(updateItem);
                        _CIRDBContext.SaveChanges();
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Lookup Item Updated Successfully." });
                    }
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested Lookup Item were not found" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
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
                    cultureCodeCultureId = 0;
                else
                    cultureCodeCultureId = cultureId;
                if (code == string.Empty || code == null) code = string.Empty;

                lookupModel.CulturalCodesList = GetCulturalCodesList(cultureCodeCultureId, code, searchCultureCode);

                if (lookupModel == null)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
                }

                return new JsonResult(new CustomResponse<LookupsModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = lookupModel });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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
                        LookupItemsText lookupModel = new LookupItemsText();

                        lookupModel.Id = Convert.ToInt64(row["Id"]);
                        lookupModel.Text = Convert.ToString(row["Text"]);
                        lookupModel.LookupItemId = Convert.ToInt64(row["LookupItemId"]);
                        lookupModel.Active = Convert.ToBoolean(row["Active"]);
                        lookupModel.CultureId = Convert.ToInt64(row["CultureId"]);
                        lookupsItemList.Add(lookupModel);
                    }
                }
                if (lookupsItemList.Count == 0)
                {
                    return new JsonResult(new CustomResponse<LookupItemsText>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }

                return new JsonResult(new CustomResponse<List<LookupItemsText>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = lookupsItemList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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
            var checkItemExist = await _CIRDBContext.LookupItemsText.Where(x => x.CultureId == cultureId && x.LookupItemId == lookupItemId).FirstOrDefaultAsync();

            if (checkItemExist != null)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
