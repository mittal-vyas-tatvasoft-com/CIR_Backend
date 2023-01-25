using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Globalconfiguration Weekends") });
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
                    _CIRDbContext.Weekends.Remove(weekend);
                    await _CIRDbContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Weekends") });
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
                    weekends.WeekendsList.Add(weekend);
                }

                IEnumerable<WeekendModel> weekendLists = weekends.WeekendsList;
                weekendLists = weekends.WeekendsList.Where(x => x.CountryName.ToLower().Contains(searchText) || x.CountryCode.ToLower().Contains(searchText) || x.DayOfWeek.ToLower().Contains(searchText));

                if (filterCountryCodeId != null && filterCountryNameId == null && searchText.IsNullOrEmpty())
                {
                    weekendLists = weekendLists.Where(x => x.CountryId == filterCountryCodeId).ToList();
                }
                if (filterCountryNameId != null && filterCountryCodeId == null && searchText.IsNullOrEmpty())
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryNameId).ToList();
                }
                if (filterCountryNameId != null && filterCountryCodeId != null && searchText.IsNullOrEmpty())
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryNameId && x.CountryId == filterCountryCodeId).ToList();
                }
                if (filterCountryCodeId != null && filterCountryNameId == null && !searchText.IsNullOrEmpty())
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryCodeId && (x.CountryName.ToLower().Contains(searchText) || x.CountryCode.ToLower().Contains(searchText) || x.DayOfWeek.ToLower().Contains(searchText))).ToList();
                }
                if (filterCountryNameId != null && filterCountryCodeId == null && !searchText.IsNullOrEmpty())
                {
                    weekendLists = weekends.WeekendsList.Where(x => x.CountryId == filterCountryNameId && (x.CountryName.ToLower().Contains(searchText) || x.CountryCode.ToLower().Contains(searchText) || x.DayOfWeek.ToLower().Contains(searchText))).ToList();
                }
                if (filterCountryNameId != null && filterCountryCodeId != null && !searchText.IsNullOrEmpty())
                {
                    weekendLists = weekendLists.Where(x => x.CountryId == filterCountryNameId && x.CountryId == filterCountryCodeId && (x.CountryName.ToLower().Contains(searchText) || x.CountryCode.ToLower().Contains(searchText) || x.DayOfWeek.ToLower().Contains(searchText))).ToList();
                }
                weekendLists = sortAscending ? weekendLists.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)) : weekendLists.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null));
                weekends.Count = weekendLists.Count();

                var sortedWeekends = weekendLists.Skip(displayStart).Take(displayLength);
                weekends.WeekendsList = sortedWeekends.ToList();
				return new JsonResult(new CustomResponse<GlobalConfigurationWeekendsModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = weekends });
			}
            catch (Exception ex)
            {
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
