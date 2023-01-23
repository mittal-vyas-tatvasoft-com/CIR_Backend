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
		[HttpPost("AddCSV")]
		public async Task<IActionResult> GetHolidayCSV(IFormFile uploadedFile)
		{
			try
			{
				var filExtension = Path.GetExtension(uploadedFile.FileName);
				var fileName = Guid.NewGuid().ToString() + filExtension;
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
				using (FileStream fs = System.IO.File.Create(filePath))
				{
					uploadedFile.CopyTo(fs);
				}
				if (filExtension == ".csv" || filExtension == ".xlsx")
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
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <param name="countryName"> word takes country name </param>
		/// <param name="countryCode"> word takes country name </param>
		/// <returns> filtered list of holidays </returns>
		[HttpGet]
		public async Task<IActionResult> GetAllHolidays(int displayLength, int displayStart, string? sortCol, string? search, int? countryCode, int? countryName, bool sortAscending = true)
		{
			try
			{
				search ??= string.Empty;
				countryCode ??= null;
				countryName ??= null;
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
		/// <param name="holidayId"></param>
		/// <returns></returns>
		[HttpGet("{HolidayId}")]
		public async Task<IActionResult> Get(long holidayId)
		{
			try
			{
				return await _globalConfigurationHolidaysService.GetHolidayById(holidayId);
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
		[HttpPut("Update")]
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
		/// This method takes roles details and update role
		/// </summary>
		/// <param name="holidayModel"></param>
		/// <returns></returns>
		[HttpPost("Create")]
		public async Task<IActionResult> Create(Holidays holidayModel)
		{
			try
			{
				return await _globalConfigurationHolidaysService.CreateOrUpdateGlobalConfigurationHolidays(holidayModel);
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
		[HttpDelete("Delete")]
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
