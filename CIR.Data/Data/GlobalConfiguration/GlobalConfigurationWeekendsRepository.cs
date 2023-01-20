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
        /// <param name="globalConfigurationWeekends"> new weekends data for weekend </param>
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
        /// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
        /// <param name="filterCountryCodeId">used to filter weekends list based on country code Id</param>
        /// <param name="filterCountryNameId">used to filter weekends list based on country name Id</param>
        /// <returns> filtered list of Weekends </returns>
        public async Task<ActionResult> GetGlobalConfigurationWeekends(int displayLength, int displayStart, string? sortCol, int? filterCountryNameId, int? filterCountryCodeId, string? search, bool sortAscending = true)
        {
            string searchText = string.Empty;
            GlobalConfigurationWeekendsModel weekends = new();
            if (search != null && search != string.Empty)
                searchText = search.ToLower();


            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try

            {
                var weekendList = sortAscending ? (from week in _CIRDbContext.Weekends
                                                   join country in _CIRDbContext.CountryCodes
                                                                                on week.CountryId equals country.Id
                                                   select new WeekendModel()
                                                   {
                                                       Id = week.Id,
                                                       CountryId = country.Id,
                                                       DayOfWeekId = week.DayOfWeekId,
                                                       CountryCode = country.Code,
                                                       CountryName = country.CountryName,
                                                   }).OrderBy(x => EF.Property<object>(x, sortCol))
                                   : (from week in _CIRDbContext.Weekends
                                      join country in _CIRDbContext.CountryCodes
                                                                   on week.CountryId equals country.Id
                                      select new WeekendModel()
                                      {
                                          Id = week.Id,
                                          CountryId = country.Id,
                                          DayOfWeekId = week.DayOfWeekId,
                                          CountryCode = country.Code,
                                          CountryName = country.CountryName,
                                      }).OrderByDescending(x => EF.Property<object>(x, sortCol));
                if (weekendList == null)
                {
                    return new JsonResult(new CustomResponse<List<WeekendModel>>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = null });
                }
                foreach (var item in weekendList)
                {

                    WeekendModel weekendModel = item;
                    switch ((DayOfWeek)item.DayOfWeekId)
                    {
                        case System.DayOfWeek.Sunday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Sunday.GetType(), System.DayOfWeek.Sunday);
                            break;
                        case System.DayOfWeek.Monday:

                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Monday.GetType(), System.DayOfWeek.Monday);
                            break;
                        case System.DayOfWeek.Tuesday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Tuesday.GetType(), System.DayOfWeek.Tuesday);
                            break;
                        case System.DayOfWeek.Wednesday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Wednesday.GetType(), System.DayOfWeek.Wednesday);
                            break;
                        case System.DayOfWeek.Thursday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Thursday.GetType(), System.DayOfWeek.Thursday);
                            break;
                        case System.DayOfWeek.Friday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Friday.GetType(), System.DayOfWeek.Friday);
                            break;
                        case System.DayOfWeek.Saturday:
                            weekendModel.DayOfWeek = Enum.GetName(System.DayOfWeek.Saturday.GetType(), System.DayOfWeek.Saturday);
                            break;
                    }
                    weekends.WeekendsList.Add(weekendModel);
                }

                IEnumerable<WeekendModel> weekendLists = weekends.WeekendsList;


                weekendLists = weekends.WeekendsList.Where(x => x.CountryName.ToLower().Contains(searchText) || x.CountryCode.ToLower().Contains(searchText) || x.DayOfWeek.ToLower().Contains(searchText));

                if (filterCountryCodeId != null)
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryCodeId).ToList();
                }
                if (filterCountryNameId != null)
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryNameId).ToList();
                }


                weekends.Count = weekendLists.Count();

                var sortedWeekends = weekendLists.Skip(displayStart).Take(displayLength);
                weekends.WeekendsList = sortedWeekends.ToList();

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
