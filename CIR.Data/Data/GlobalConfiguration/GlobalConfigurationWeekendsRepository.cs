using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// This method is used by create method of globalconfiguration weekend
        /// </summary>
        /// <param name="weekends"> new weekends data for weekend </param>
        /// <returns> Ok status if its valid else unprocessable </returns>
        public async Task<IActionResult> CreateGlobalConfigurationWeekendsWeekends(GlobalConfigurationWeekends globalConfigurationWeekends)
        {
            try
            {
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
                var Weekend = _CIRDbContext.Weekends.FirstOrDefault(x => x.Id == id);
                if (Weekend != null)
                {
                    _CIRDbContext.Weekends.Remove(Weekend);
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
        public async Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true)
        {
            string SearchText = string.Empty;
            GlobalConfigurationWeekendsModel Weekends = new();
            if (search != null && search != string.Empty)
                SearchText = search.ToLower();


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
                                       CountryName = country.CountryName
                                   });

                foreach (var item in weekendList)
                {
                    WeekendModel weekend = item;
                    weekend.DayOfWeek = GetDayOfWeek(item.DayOfWeekId);
                    Weekends.WeekendsList.Add(weekend);
                }
                
                IEnumerable<WeekendModel> WeekendLists = Weekends.WeekendsList;
                WeekendLists = Weekends.WeekendsList.Where(x => x.CountryName.ToLower().Contains(SearchText) || x.CountryCode.ToLower().Contains(SearchText) || x.DayOfWeek.ToLower().Contains(SearchText));

                if (filterCountryCodeId != null)
                {
                    WeekendLists = WeekendLists.Where(x => x.CountryId == filterCountryCodeId).ToList();
                }
                if (filterCountryNameId != null)
                {
                    WeekendLists = WeekendLists.Where(x => x.CountryId == filterCountryNameId).ToList();
                }
                WeekendLists = sortAscending ? WeekendLists.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)) : WeekendLists.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null));
                Weekends.Count = WeekendLists.Count();

                var sortedData = WeekendLists.Skip(displayStart).Take(displayLength);
                Weekends.WeekendsList = sortedData.ToList();

                return new JsonResult(new CustomResponse<GlobalConfigurationWeekendsModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = Weekends });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        private string GetDayOfWeek(long dayOfWeekId)
        {
            if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Sunday)
            {
                return Enum.GetName(System.DayOfWeek.Sunday.GetType(), System.DayOfWeek.Sunday);
            }
            else if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Monday)
            {

                return Enum.GetName(System.DayOfWeek.Monday.GetType(), System.DayOfWeek.Monday); 
            }
            else if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Tuesday)
            {
                    return Enum.GetName(System.DayOfWeek.Tuesday.GetType(), System.DayOfWeek.Tuesday);
            }
            else if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Wednesday)
            {
                return Enum.GetName(System.DayOfWeek.Wednesday.GetType(), System.DayOfWeek.Wednesday);
            }
            else if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Thursday)
            {
                return Enum.GetName(System.DayOfWeek.Thursday.GetType(), System.DayOfWeek.Thursday);
            }
            else if ((DayOfWeek)dayOfWeekId == System.DayOfWeek.Friday)
            {
                return Enum.GetName(System.DayOfWeek.Friday.GetType(), System.DayOfWeek.Friday);
            }
            else
            {
                return Enum.GetName(System.DayOfWeek.Saturday.GetType(), System.DayOfWeek.Saturday);
            }
        }
        #endregion
    }
}
