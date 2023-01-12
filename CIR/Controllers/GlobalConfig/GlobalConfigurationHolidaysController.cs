using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.Common;
using CIR.Core.Interfaces.GlobalConfig;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CIR.Controllers.GlobalConfig
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
		/// <param name="Holiday"> this object contains different parameters as details of a user </param>
		/// <returns > created user </returns>
		[HttpPost]
		public async Task<IActionResult> GetHolidayCSV(IFormFile uploadedfile)
		{
			try
			{
				var fileextension = Path.GetExtension(uploadedfile.FileName);
				var filename = Guid.NewGuid().ToString() + fileextension;
				var filepath = Path.Combine(Directory.GetCurrentDirectory(), filename);
				using (FileStream fs = System.IO.File.Create(filepath))
				{
					uploadedfile.CopyTo(fs);
				}
				if (fileextension == ".csv" || fileextension == ".xlsx")
				{
					using (var reader = new StreamReader(filepath))
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
	}
	#endregion
}
