﻿using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfig
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
        /// This method is used by create method of globalconfiguration weekend
        /// </summary>
        /// <param name="weekends"> new weekends data for weekend </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends globalConfigurationWeekends)
        {
            try
            {
                if (globalConfigurationWeekends.CountryId == 0 || globalConfigurationWeekends.DayOfWeekId == 0)
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Please enter valid Data" });
                }
                GlobalConfigurationWeekends globalConfigWeeknds = new()
                {

                    CountryId = globalConfigurationWeekends.CountryId,
                    DayOfWeekId = globalConfigurationWeekends.DayOfWeekId,
                };

                _CIRDbContext.Weekends.Add(globalConfigWeeknds);

                await _CIRDbContext.SaveChangesAsync();
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Globalconfiguration weekends saved successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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
                    _CIRDbContext.Weekends.Remove(weekend);
                    await _CIRDbContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Deleted, Data = "Weekends Deleted Successfully." });
                }
                else
                {
                    return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = true, Message = HttpStatusCodesMessages.NotFound, Data = "Weekends id not found." });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }

        }

        /// <summary>
        /// This method retuns filtered Weekends list using LINQ
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in Weekends table </param>
        /// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
        /// <returns> filtered list of Weekends </returns>
        public async Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
        {
            GlobalConfigurationWeekendsModel weekends = new();


            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try

            {
                var weekendList = (from week in _CIRDbContext.Weekends
                                   join country in _CIRDbContext.CountryCodes
                                                                on week.CountryId equals country.Id
                                   select new WeekendModel()
                                   {
                                       Id = week.Id,
                                       CountryId = country.Id,
                                       DayOfWeekId = week.DayOfWeekId,
                                       CountryCode = country.Code,
                                       CountryName = country.CountryName,
                                   }).OrderBy(x => EF.Property<object>(x, sortCol));
                foreach (var item in weekendList)
                {
                    WeekendModel weekendModel = item;
                    switch ((DayOfWeek)item.DayOfWeekId)
                    {
                        case System.DayOfWeek.Sunday:
                            weekendModel.DayOfWeek = "Sunday";
                            break;
                        case System.DayOfWeek.Monday:

                            weekendModel.DayOfWeek = "Monday";
                            break;
                        case System.DayOfWeek.Tuesday:
                            weekendModel.DayOfWeek = "Tuesday";
                            break;
                        case System.DayOfWeek.Wednesday:
                            weekendModel.DayOfWeek = "Wednesday";
                            break;
                        case System.DayOfWeek.Thursday:
                            weekendModel.DayOfWeek = "Thursday";
                            break;
                        case System.DayOfWeek.Friday:
                            weekendModel.DayOfWeek = "Friday";
                            break;
                        case System.DayOfWeek.Saturday:
                            weekendModel.DayOfWeek = "SaturDay";
                            break;
                    }
                    weekends.WeekendsList.Add(weekendModel);
                }

                IQueryable<WeekendModel> weekendLists = weekends.WeekendsList.AsQueryable();

                weekends.Count = weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search) || x.DayOfWeek.Contains(search)).Count();

                weekendLists = sortAscending ? weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search) || x.DayOfWeek.Contains(search)).AsQueryable()
                                     : weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search) || x.DayOfWeek.Contains(search)).AsQueryable();

                var sortedData = weekendLists.Skip(displayStart).Take(displayLength);
                weekends.WeekendsList = sortedData.ToList();

                return new JsonResult(new CustomResponse<GlobalConfigurationWeekendsModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = weekends });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
