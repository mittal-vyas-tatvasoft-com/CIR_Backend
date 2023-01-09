using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Data.Data.GlobalConfig
{
	public class HolidaysRepository : ControllerBase, IHolidaysRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public HolidaysRepository(CIRDbContext context)
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
		public async Task<IActionResult> CreateOrUpdateHoliday(Holidays holiday)
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
			if (holiday.Id != null)
			{
				return Ok(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
			}
			return UnprocessableEntity(new CustomResponse<Holidays>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
		}
		#endregion
	}
}
