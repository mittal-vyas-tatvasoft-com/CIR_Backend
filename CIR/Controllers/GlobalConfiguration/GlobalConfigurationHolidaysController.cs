using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.Common;
using CIR.Core.Interfaces.GlobalConfiguration;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CIR.Controllers.GlobalConfiguration
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class GlobalConfigurationHolidaysController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationHolidaysService _globalConfigurationHolidaysService;
		private readonly ICsvService _csvService;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationHolidaysController(IGlobalConfigurationHolidaysService globalConfigurationHolidaysService, ICsvService csvService)
		{
			_globalConfigurationHolidaysService = globalConfigurationHolidaysService;
			_csvService = csvService;
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method takes holiday details as parameters and creates user and returns that user
		/// </summary>
		/// <param name="uploadedFile"> this object contains different parameters as details of a user </param>
		/// <returns > created user </returns>
		[HttpPost]
		public async Task<IActionResult> GetHolidayCSV(IFormFile uploadedFile)
		{
			try
			{
				var fileExtension = Path.GetExtension(uploadedFile.FileName);
				var fileName = Guid.NewGuid().ToString() + fileExtension;
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
				using (FileStream fs = System.IO.File.Create(filePath))
				{
					uploadedFile.CopyTo(fs);
				}
				if (fileExtension == ".csv" || fileExtension == ".xlsx")
				{
					using (var reader = new StreamReader(filePath))
					using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
					{
						var records = csv.GetRecords<Holidays>();
						foreach (var record in records)
						{
							if (record == null)
							{
								break;
							}
							await _globalConfigurationHolidaysService.CreateOrUpdateGlobalConfigurationHolidays(record);
						}
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
					}
				}
				else
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "select only .xlsx or .csv file" });
				}
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
		/// <param name="countryCode"> used to filter table based on country code</param>
		/// <param name="countryName">used to filter table based on country name</param>
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of holidays </returns>
		[HttpGet]
		public async Task<IActionResult> GetAllHolidays(int displayLength, int displayStart, string? sortCol, string? search, string? countryCode, string? countryName, bool sortAscending = true)
		{
			try
			{
				search ??= string.Empty;
				countryCode ??= string.Empty;
				countryName ??= string.Empty;
				return await _globalConfigurationHolidaysService.GetGlobalConfigurationHolidays(displayLength, displayStart, sortCol, search, countryCode, countryName, sortAscending);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes holiday id and return holiday
		/// </summary>
		/// <param name="HolidayId"></param>
		/// <returns></returns>
		[HttpGet("{HolidayId}")]
		public async Task<IActionResult> Get(long HolidayId)
		{
			try
			{
				return await _globalConfigurationHolidaysService.GetHolidayById(HolidayId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes roles details and update role
		/// </summary>
		/// <param name="holidayModel"></param>
		/// <returns></returns>
		[HttpPut]
		public async Task<IActionResult> Update(Holidays holidayModel)
		{
			try
			{
				return await _globalConfigurationHolidaysService.UpdateHoliday(holidayModel);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes holiday details and delete holiday
		/// </summary>
		/// <param name="holidayId"></param>
		/// <returns></returns>
		[HttpDelete]
		public async Task<IActionResult> Delete(long holidayId)
		{
			try
			{
				if (holidayId > 0)
				{
					return await _globalConfigurationHolidaysService.DeleteHolidays(holidayId);
				}
				return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = ex });
			}
		}
	}
	#endregion
}
