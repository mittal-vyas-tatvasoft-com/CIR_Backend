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
    public class GlobalConfigurationHolidaysRepository : ControllerBase, IGlobalConfigurationHolidaysRepository
    {
        #region PROPERTIES
        private readonly CIRDbContext _CIRDbContext;
        #endregion

        #region CONSTRUCTOR
        public GlobalConfigurationHolidaysRepository(CIRDbContext context)
        {
            _CIRDbContext = context ??
                throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method is used by create method and update method of holiday controller
        /// </summary>
        /// <param name="holiday"> new holiday data or update data for holiday </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateOrUpdateGlobalConfigurationHolidays(Holidays holiday)
        {
            try
            {
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", holiday.Id);
                        parameters.Add("@CountryId", holiday.CountryId);
                        parameters.Add("@Date", holiday.Date);
                        parameters.Add("@Description", holiday.Description);
                        result = connection.Execute("spCreateOrUpdateGlobalConfigurationHolidays", parameters, commandType: CommandType.StoredProcedure);

                    }
                }
                if (result != 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Configuration Holiday") });
                }
                return new JsonResult(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }
        /// <summary>
        /// This method retuns filtered holidays list using SP
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="countryCodeId"> used to filter table based on country code</param>
        /// <param name="countryNameId">used to filter table based on country name</param>
        /// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
        /// <returns></returns>        
        public async Task<IActionResult> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, int countryCodeId, int countryNameId, bool sortAscending = true)
        {
            HolidayViewModel holidayList = new();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }
            try
            {
                List<HolidayModel> sortedHolidays;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@DisplayLength", displayLength);
                        parameters.Add("@DisplayStart", displayStart);
                        parameters.Add("@SortCol", sortCol);
                        parameters.Add("@Search", search);
                        parameters.Add("@SortDir", sortAscending);
                        parameters.Add("@CountryCodeId", countryCodeId);
                        parameters.Add("@CountryNameId", countryNameId);
                        sortedHolidays = connection.Query<HolidayModel>("spGetGlobalConfigurationHolidays", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                if (sortedHolidays.Count > 0)
                {
                    sortedHolidays = sortedHolidays.ToList();
                    holidayList.Count = sortedHolidays[0].TotalCount;
                    holidayList.HolidayList = sortedHolidays;
                }
                else
                {
                    holidayList.Count = 0;
                    holidayList.HolidayList = sortedHolidays;
                }

                return new JsonResult(new CustomResponse<HolidayViewModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = holidayList });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// fetches holidays based on input holiday id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> holiday or null holiday if not found </returns>
        public async Task<IActionResult> GetHolidayById(long id)
        {
            try
            {
                if (id == null)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
                }
                Holidays holidayDetail;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@HolidayId", id);
                        holidayDetail = connection.Query<Holidays>("spGetHolidayById", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    }
                }
                if (holidayDetail == null)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
                }
                return new JsonResult(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = holidayDetail });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }

        /// <summary>
        /// This method takes a delete holiday 
        /// </summary>
        /// <param name="holidayId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteHolidays(long holidayId)
        {
            try
            {
                if (holidayId == null)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute() });
                }
                var result = 0;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@HolidayId", holidayId);
                        result = connection.Execute("spDeleteHolidays", parameters, commandType: CommandType.StoredProcedure);
                    }
                }
                if (result != 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Deleted, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Holiday") });
                }
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
            }
        }
        #endregion
    }
}
