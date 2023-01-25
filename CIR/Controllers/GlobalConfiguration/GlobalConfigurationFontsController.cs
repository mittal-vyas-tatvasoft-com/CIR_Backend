using CIR.Common.CustomResponse;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.GlobalConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfiguration
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class GlobalConfigurationFontsController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationFontsServices _globalConfigurationFontsServices;
		#endregion
		#region CONSTRUCTOR
		public GlobalConfigurationFontsController(IGlobalConfigurationFontsServices globalConfigurationFontsServices)
		{
			_globalConfigurationFontsServices = globalConfigurationFontsServices;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes a get globalconfiguration fonts list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return await _globalConfigurationFontsServices.GetGlobalConfigurationFonts();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create globalconfiguration fonts
		/// </summary>
		/// <param name="globalConfigurationFonts"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<GlobalConfigurationFonts> globalConfigurationFonts)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationFontsServices.CreateOrUpdateGlobalConfigurationFonts(globalConfigurationFonts);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes a update globalconfiguration fonts
		/// </summary>
		/// <param name="globalConfigurationFonts"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<GlobalConfigurationFonts> globalConfigurationFonts)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationFontsServices.CreateOrUpdateGlobalConfigurationFonts(globalConfigurationFonts);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}


		#endregion
	}
}
