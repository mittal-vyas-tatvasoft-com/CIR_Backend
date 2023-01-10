using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfig;
using CIR.Core.Interfaces.GlobalConfig;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfig
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class FontsController : ControllerBase
	{
		#region PROPERTIES
		private readonly IFontServices _fontServices;
		#endregion
		#region CONSTRUCTOR
		public FontsController(IFontServices fontServices)
		{
			_fontServices = fontServices;
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method fetches all fonts
		/// </summary>
		/// <returns> Fonts </returns>

		[HttpGet]
		public async Task<IActionResult> GetAllFonts()
		{
			try
			{
				return await _fontServices.GetAllFonts();
			}
			catch (Exception Ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = Ex });
			}
		}

		/// <summary>
		/// This method takes fonts details as parameters and creates fonts and returns that font
		/// </summary>
		/// <param name="fonts"> this object contains different parameters as details of a fonts </param>
		/// <returns > created fonts </returns>

		[HttpPost]
		public async Task<IActionResult> Create(List<GlobalConfigurationFonts> fonts)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _fontServices.CreateOrUpdateFonts(fonts);
				}
				catch (Exception Ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = Ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}

		/// <summary>
		/// This method takes fonts details and updates the font
		/// </summary>
		/// <param name="fonts"> this object contains different parameters as details of a fonts	</param>
		/// <returns> updated fonts </returns>
		[HttpPut]
		public async Task<IActionResult> Update(List<GlobalConfigurationFonts> fonts)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _fontServices.CreateOrUpdateFonts(fonts);
				}
				catch (Exception Ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = Ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}


		#endregion
	}
}
