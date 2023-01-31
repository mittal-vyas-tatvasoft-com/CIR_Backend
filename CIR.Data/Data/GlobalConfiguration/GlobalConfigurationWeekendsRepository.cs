using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CIR.Data.Data.GlobalConfiguration
{
    public class GlobalConfigurationWeekendsRepository : IGlobalConfigurationWeekendsRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _CIRDbContext;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationWeekendsRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method is used by check countrywise weekend already exist or not
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="dayOfWeekId"></param>
        /// <returns></returns>
        public async Task<bool> CountryWiseWeekendsExists(long countryId, long dayOfWeekId)
        {

            var weekendModel = await _CIRDbContext.Weekends.Where(x => x.CountryId == countryId && x.DayOfWeekId == dayOfWeekId).FirstOrDefaultAsync();

            if (weekendModel != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used by create method of globalconfiguration weekend
        /// </summary>
        /// <param name="globalConfigurationWeekends"> new weekends data for weekend </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateGlobalConfigurationWeekends(GlobalConfigurationWeekends globalConfigurationWeekends)
        {
            try
            {
                if (globalConfigurationWeekends.CountryId == 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = "Please enter valid Data" });
                }
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {

                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@CountryId", globalConfigurationWeekends.CountryId);
                        parameters.Add("@DayOfWeekId", globalConfigurationWeekends.DayOfWeekId);
                        result = connection.Execute("spAddGlobalConfigurationWeekends", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result != 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Globalconfiguration Weekends") });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = "Something went wrong!" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }


        /// <summary>
        /// This method used by delete globalconfiguration weekend
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteGlobalConfigurationWeekend(int id)
        {
            try
            {
                var weekend = _CIRDbContext.Weekends.FirstOrDefault(x => x.Id == id);
                if (weekend != null)
                {
                    using (DbConnection dbConnection = new DbConnection())
                    {

                        using (var connection = dbConnection.Connection)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("@Id", weekend.Id);
                            Convert.ToString(connection.ExecuteScalar("spDeleteGlobalConfigurationWeekend", parameters, commandType: CommandType.StoredProcedure));
                            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Weekends") });
                        }
                    }
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgIdNotFound, "Weekend") });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method retuns filtered Weekends list using LINQ
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in Weekends table </param>
        /// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
        /// <param name="filterCountryCodeId">used to filter weekends list based on country code Id</param>
        /// <param name="filterCountryNameId">used to filter weekends list based on country name Id</param>
        /// <returns> filtered list of Weekends </returns>
        public async Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true)
        {
            GlobalConfigurationWeekendsModel weekendList = new();
            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }
            try
            {
                List<WeekendModel> sortWeekendData;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("DisplayLength", displayLength);
                        parameters.Add("DisplayStart", displayStart);
                        parameters.Add("SortCol", sortCol);
                        parameters.Add("Search", search);
                        parameters.Add("SortDir", sortAscending);
                        parameters.Add("FilterCountryNameId", filterCountryNameId);
                        parameters.Add("FilterCountryCodeId", filterCountryCodeId);
                        sortWeekendData = connection.Query<WeekendModel>("spGetGlobalConfigurationWeekends", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                if (sortWeekendData.Count > 0)
                {
                    sortWeekendData = sortWeekendData.ToList();
                    weekendList.Count = sortWeekendData[0].TotalCount;
                    weekendList.WeekendsList = sortWeekendData;
                }
                else
                {
                    weekendList.Count = 0;
                    weekendList.WeekendsList = sortWeekendData;
                }
                return new JsonResult(new CustomResponse<GlobalConfigurationWeekendsModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = weekendList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }

        }

        #endregion
    }
}
