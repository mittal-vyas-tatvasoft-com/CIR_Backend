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

				Holidays newholiday = new()
				{
					Id = holiday.Id,
					CountryId = holiday.CountryId,
					Date = holiday.Date,
					Description = holiday.Description,
				};

				if (holiday.Id > 0)
				{
					_CIRDbContext.Holidays.Update(newholiday);
				}
				else
				{
					_CIRDbContext.Holidays.Add(newholiday);
				}

				await _CIRDbContext.SaveChangesAsync();
				if (holiday.Id != 0)
				{
					return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "GlobalConfiguration holiday saved successfully." });
				}
				return UnprocessableEntity(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}
		/// <summary>
		/// This method is used by get globalconfiguration holidays list
		/// </summary>
		/// <returns></returns>
		public async Task<HolidayModel> GetGlobalConfigurationHolidays(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
		{
			HolidayModel holiday = new();
			IQueryable<Holidays> iQHoliday = holiday.HolidayList.AsQueryable();

			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}
			try
			{
				holiday.Count = _CIRDbContext.Holidays.Where(y => y.Description.Contains(search)).Count();

				iQHoliday = sortAscending ? _CIRDbContext.Holidays.Where(y => y.Description.Contains(search)).OrderBy(x => EF.Property<object>(x, sortCol)).AsQueryable()
									 : _CIRDbContext.Holidays.Where(y => y.Description.Contains(search)).OrderByDescending(x => EF.Property<object>(x, sortCol)).AsQueryable();

				var sortedData = await iQHoliday.Skip(displayStart).Take(displayLength).ToListAsync();
				holiday.HolidayList = sortedData;

				return holiday;
			}
			catch (Exception ex)
			{
				return holiday;
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
				var holidayList = await _CIRDbContext.Holidays.Where(x => x.Id == id).FirstOrDefaultAsync();
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
		/// <param name="HolidayId"></param>
		/// <returns></returns>
		public async Task<IActionResult> UpdateHoliday(Holidays HolidayModel)
		{
			try
			{
				if (HolidayModel.Id != 0)
				{
					Holidays newholiday = new Holidays()
					{
						Id = HolidayModel.Id,
						CountryId = HolidayModel.CountryId,
						Date = HolidayModel.Date,
						Description = HolidayModel.Description
					};
					_CIRDbContext.Holidays.Update(newholiday);
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
