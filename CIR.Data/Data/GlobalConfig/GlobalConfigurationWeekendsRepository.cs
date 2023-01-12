using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.GlobalConfig;
using CIR.Core.ViewModel.GlobalConfig;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.GlobalConfig
{
    public class GlobalConfigurationWeekendsRepository :  IGlobalConfigurationWeekendsRepository
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
        /// This method is used by create method of weekends controller
        /// </summary>
        /// <param name="weekends"> new weekends data for weekend </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends weekends)
        {
            try
            {
                GlobalConfigurationWeekends newWeekends = new()
                {
                    
                    CountryId = weekends.CountryId,
                    DayOfWeekId = weekends.DayOfWeekId,
                };
            
                    _CIRDbContext.Weekends.Add(newWeekends);

                await _CIRDbContext.SaveChangesAsync();
                 return new JsonResult(new CustomResponse<GlobalConfigurationWeekends>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }


        /// <summary>
        /// this metohd updates a column value and disables Weekends
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<IActionResult> DeleteGlobalConfigurationWeekend(int id)
        {
            try
            {
                GlobalConfigurationWeekends weekend= _CIRDbContext.Weekends.FirstOrDefault(x => x.Id == id);
                if (weekend!= null)
                {
                    _CIRDbContext.Weekends.Remove(weekend);
                    await _CIRDbContext.SaveChangesAsync();
                    return new JsonResult(new CustomResponse<GlobalConfigurationWeekends>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent});
                }
                else
                {
                    return new JsonResult(new CustomResponse<GlobalConfigurationWeekends>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = true, Message = HttpStatusCodesMessages.NotFound});
                }

            }
            catch
            {
                    return new JsonResult(new CustomResponse<GlobalConfigurationWeekends>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound});
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


        public async Task<ActionResult> GetAllGlobalConfigurationWeekends(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
        {
            GlobalConfigurationWeekendsModel weekends = new();
            

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try

            {


                var WeekendList = (from Week in _CIRDbContext.Weekends
                                   join country in _CIRDbContext.CountryCodes
                                                                on Week.CountryId equals country.Id
                                   select new WeekendModel()
                                   {
                                       Id = Week.Id,
                                       CountryId = country.Id,
                                       DayOfWeekId = Week.DayOfWeekId,
                                       CountryCode = country.Code,
                                       CountryName = country.CountryName,
                                   }).OrderBy(x => EF.Property<object>(x, sortCol));
                foreach (var item in WeekendList)
                {
                    WeekendModel model = item;
                    switch ((DayOfWeek)item.DayOfWeekId)
                    {
                        case System.DayOfWeek.Sunday:
                            model.DayOfWeek = "Sunday";
                            break;
                        case System.DayOfWeek.Monday:

                            model.DayOfWeek = "Monday";
                            break;
                        case System.DayOfWeek.Tuesday:
                            model.DayOfWeek = "Tuesday";
                            break;
                        case System.DayOfWeek.Wednesday:
                            model.DayOfWeek = "Wednesday";
                            break;
                        case System.DayOfWeek.Thursday:
                            model.DayOfWeek = "Thursday";
                            break;
                        case System.DayOfWeek.Friday:
                            model.DayOfWeek = "Friday";
                            break;
                        case System.DayOfWeek.Saturday:
                            model.DayOfWeek = "SaturDay";
                            break;
                    }
                    weekends.WeekendsList.Add(model);
                }

                IQueryable<WeekendModel> temp = weekends.WeekendsList.AsQueryable();

                weekends.Count = weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search)||x.DayOfWeek.Contains(search)).Count();

                temp = sortAscending ? weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search) || x.DayOfWeek.Contains(search)).AsQueryable()
                                     : weekends.WeekendsList.Where(x => x.CountryName.Contains(search) || x.CountryCode.Contains(search) || x.DayOfWeek.Contains(search)).AsQueryable();

                var sortedData = temp.Skip(displayStart).Take(displayLength);
                weekends.WeekendsList = sortedData.ToList();
              
                return new JsonResult(new CustomResponse<GlobalConfigurationWeekendsModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success,Data= weekends});
               
            }
            catch(Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
