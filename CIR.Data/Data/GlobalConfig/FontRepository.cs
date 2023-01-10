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
		public async Task<IActionResult> CreateOrUpdateFont(List<GlobalConfigurationFonts> fonts)
		{
			if (fonts != null)
			{
				foreach (var item in fonts)
				{
					if (item.Id != 0)
					{
						GlobalConfigurationFonts font = new GlobalConfigurationFonts()
						{
							Id = item.Id,
							Name = item.Name,
							Enabled = item.Enabled,
							FontFamily = item.FontFamily,
							IsDefault = item.IsDefault
						};
						_CIRDBContext.GlobalConfigurationFonts.Update(font);
					}
					else
					{
						GlobalConfigurationFonts font = new GlobalConfigurationFonts()
						{
							Name = item.Name,
							Enabled = item.Enabled,
							FontFamily = item.FontFamily,
							IsDefault = item.IsDefault
						};
						_CIRDBContext.GlobalConfigurationFonts.Add(font);

					}
				}
				_CIRDBContext.SaveChanges();
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success });
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest });
		}
		#endregion


	}
}
