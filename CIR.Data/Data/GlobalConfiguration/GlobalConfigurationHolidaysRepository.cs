using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Data;
using System.Threading.Tasks;
using CIR.Common.Enums;
using CIR.Common.Helper;

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
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Configuration Holiday") });
				}
				if (holiday.Id == 0)
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
			HolidayViewModel holiday = new();
			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}
			try
			{
				List<HolidayModel> sortedHolidayData;
				using (DbConnection dbConnection = new DbConnection())
				{
					using (var connection = dbConnection.Connection)
					{
						DynamicParameters parameters = new DynamicParameters();
						parameters.Add("Search", search);
						sortedHolidayData = connection.Query<HolidayModel>("spGetHolidayDetailLists", parameters, commandType: CommandType.StoredProcedure).ToList();
					}
				}
				if (countryCodeId != 0)
				{
					sortedHolidayData = sortedHolidayData.Where(x => x.CountryId == countryCodeId).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
				}
				if (countryNameId != 0)
				{
					sortedHolidayData = sortedHolidayData.Where(x => x.CountryId == countryNameId).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
				}
				sortedHolidayData = sortedHolidayData.ToList();
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
				return new JsonResult(new CustomResponse<HolidayViewModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = holiday });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// fetches holidays based on input holiday id
		/// </summary>
		/// <param name="holidayId"></param>
		/// <returns> holiday or null holiday if not found </returns>
		public async Task<IActionResult> GetHolidayById(long holidayId)
		{
			try
			{
				var holidayList = await _CIRDbContext.Holidays.Where(x => x.Id == holidayId).FirstOrDefaultAsync();
				if (holidayList == null)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = holidayList });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
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
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Holiday") });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgUpdatingDataError, "Holiday") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
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
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Deleted, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Holiday") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
