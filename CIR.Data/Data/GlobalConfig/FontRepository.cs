using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfig
{
	public class FontRepository : ControllerBase, IFontRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public FontRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS
		/// <summary>
		/// fetches all fonts 
		/// </summary>
		/// <returns> fonts or null fonts if no fonts </returns>
		public async Task<IActionResult> GetAllFonts()
		{
			var fonts = await _CIRDBContext.GlobalConfigurationFonts.Select(x => new GlobalConfigurationFonts()
			{
				Id = x.Id,
				Name = x.Name,
				Enabled = x.Enabled,
				FontFamily = x.FontFamily,
				IsDefault = x.IsDefault
			}).ToListAsync();
			return new JsonResult(new CustomResponse<List<GlobalConfigurationFonts>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = fonts });
		}

		/// <summary>
		/// This method is used by create method and update method of fonts controller
		/// </summary>
		/// <param name="fonts"> new fonts data or update data for fonts </param>
		/// <returns> Ok status if its valid else unprocessable </returns>
		public async Task<IActionResult> CreateOrUpdateFont(GlobalConfigurationFonts fonts)
		{
			GlobalConfigurationFonts newfont = new()
			{
				Id = fonts.Id,
				Name = fonts.Name,
				Enabled = fonts.Enabled,
				FontFamily = fonts.FontFamily,
				IsDefault = fonts.IsDefault
			};

			if (fonts.Id > 0)
			{
				_CIRDBContext.GlobalConfigurationFonts.Update(newfont);
			}
			else
			{
				_CIRDBContext.GlobalConfigurationFonts.Add(newfont);
			}
			await _CIRDBContext.SaveChangesAsync();

			if (fonts.Id != null)
			{
				return new JsonResult(new CustomResponse<GlobalConfigurationFonts>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
			}
			return new JsonResult(new CustomResponse<GlobalConfigurationFonts>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
		}
		#endregion


	}
}
