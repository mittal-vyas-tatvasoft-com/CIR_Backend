using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
				Holidays newHoliday = new()
				{
					Id = holiday.Id,
					CountryId = holiday.CountryId,
					Date = holiday.Date,
					Description = holiday.Description,
				};

				if (holiday.Id > 0)
				{
					_CIRDbContext.Holidays.Update(newHoliday);
				}
				else
				{
					_CIRDbContext.Holidays.Add(newHoliday);
				}

				await _CIRDbContext.SaveChangesAsync();
				if (holiday.Id != 0)
				{
					return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration holiday saved successfully." });
				}
				if (holiday.Id == 0)
				{
					return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration holiday created successfully." });
				}
				return UnprocessableEntity(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}
		/// <summary>
		/// This method retuns filtered holidays list using SP
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
		/// <param name="countryName"> word takes country name </param>
		/// <param name="countryCode"> word takes country name </param>
		/// <returns> filtered list of holidays </returns>
		public async Task<IActionResult> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, string? countryCode, string? countryName, bool sortAscending = true)
		{
			HolidayViewModel holiday = new();
			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}
			try
			{
				holiday.Count = _CIRDbContext.Holidays.Where(x => x.Description.Contains(search)).Count();

				var sortedHolidayData = await (from holidaydata in _CIRDbContext.Holidays
											   join countrydata in _CIRDbContext.CountryCodes
											   on holidaydata.CountryId equals countrydata.Id

											   select new HolidayModel()
											   {
												   Id = holidaydata.Id,
												   CountryId = holidaydata.CountryId,
												   Code = countrydata.Code,
												   CountryName = countrydata.CountryName,
												   Date = holidaydata.Date,
												   Description = holidaydata.Description,
											   }).ToListAsync();
				if (countryCode != "")
				{
					sortedHolidayData = sortedHolidayData.Where(y => y.Code == countryCode).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
				}
				if (countryName != "")
				{
					sortedHolidayData = sortedHolidayData.Where(y => y.CountryName == countryName).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
				}

				sortedHolidayData = sortedHolidayData.Where(y => y.Description.Contains(search) || y.CountryName.Contains(search) || y.Code.Contains(search)).ToList();
				holiday.Count = sortedHolidayData.Count();

				if (sortAscending)
				{
					sortedHolidayData = sortedHolidayData.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				else
				{
					sortedHolidayData = sortedHolidayData.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
				}
				holiday.HolidayList = sortedHolidayData;
				return new JsonResult(new CustomResponse<HolidayViewModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = holiday });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}

		/// <summary>
		/// fetches holidays based on input holiday id
		/// </summary>
		/// <param name="holidayid"></param>
		/// <returns> holiday or null holiday if not found </returns>
		public async Task<IActionResult> GetHolidayById(long holidayId)
		{
			try
			{
				var holidayList = await _CIRDbContext.Holidays.Where(x => x.Id == holidayId).FirstOrDefaultAsync();
				if (holidayList == null)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound });
				}
				return new JsonResult(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = holidayList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes a holiday data and update it
		/// </summary>
		/// <param name="holidayModel"></param>
		/// <returns></returns>
		public async Task<IActionResult> UpdateHoliday(Holidays holidayModel)
		{
			try
			{
				if (holidayModel.Id != 0)
				{
					Holidays newHoliday = new Holidays()
					{
						Id = holidayModel.Id,
						CountryId = holidayModel.CountryId,
						Date = holidayModel.Date,
						Description = holidayModel.Description
					};
					_CIRDbContext.Holidays.Update(newHoliday);
					await _CIRDbContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "holiday saved successfully" });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error occurred while updating holidays" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}

		/// <summary>
		/// This method takes a delete holiday 
		/// </summary>
		/// <param name="holidayId"></param>
		/// <returns></returns>
		public async Task<IActionResult> DeleteHolidays(long holidayId)
		{
			var holiday = new Holidays() { Id = holidayId };
			try
			{
				_CIRDbContext.Holidays.Remove(holiday);
				await _CIRDbContext.SaveChangesAsync();
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Deleted, Data = "Holiday Deleted Successfully" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}
		#endregion
	}
}
