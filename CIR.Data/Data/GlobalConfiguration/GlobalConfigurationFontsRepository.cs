using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIR.Data.Data.GlobalConfiguration
{
	public class GlobalConfigurationFontsRepository : ControllerBase, IGlobalConfigurationFontsRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDBContext;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationFontsRepository(CIRDbContext context)
		{
			_CIRDBContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method is used by get globalconfiguration fonts list
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetGlobalConfigurationFonts()
		{
			try
			{
				var fonts = await _CIRDBContext.GlobalConfigurationFonts.Select(x => new GlobalConfigurationFonts()
				{
					Id = x.Id,
					Name = x.Name,
					Enabled = x.Enabled,
					FontFamily = x.FontFamily,
					IsDefault = x.IsDefault
				}).ToListAsync();

				if (fonts.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<GlobalConfigurationFonts>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<GlobalConfigurationFonts>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = fonts });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method used by create or update globalconfiguration fonts
		/// </summary>
		/// <param name="globalConfigurationFonts"></param>
		/// <returns></returns>
		public async Task<IActionResult> CreateOrUpdateGlobalConfigurationFonts(List<GlobalConfigurationFonts> globalConfigurationFonts)
		{
			try
			{
				if (globalConfigurationFonts.Any(x => x.Name == "string" || x.Name == string.Empty))
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgEnterValidData });
				}
				if (globalConfigurationFonts != null)
				{
					foreach (var item in globalConfigurationFonts)
					{
						if (item.Id != 0)
						{
							GlobalConfigurationFonts globalConfigFonts = new GlobalConfigurationFonts()
							{
								Id = item.Id,
								Name = item.Name,
								Enabled = item.Enabled,
								FontFamily = item.FontFamily,
								IsDefault = item.IsDefault
							};
							_CIRDBContext.GlobalConfigurationFonts.Update(globalConfigFonts);
						}
						else
						{
							GlobalConfigurationFonts globalConfigFonts = new GlobalConfigurationFonts()
							{
								Name = item.Name,
								Enabled = item.Enabled,
								FontFamily = item.FontFamily,
								IsDefault = item.IsDefault
							};
							_CIRDBContext.GlobalConfigurationFonts.Add(globalConfigFonts);

						}
					}
					await _CIRDBContext.SaveChangesAsync();
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Global Configuration Fonts") });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion


	}
}
