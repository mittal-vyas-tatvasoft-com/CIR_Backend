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
	public class GlobalConfigurationStylesController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationStylesService _globalConfigurationStylesService;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationStylesController(IGlobalConfigurationStylesService globalConfigurationStylesService)
		{
			_globalConfigurationStylesService = globalConfigurationStylesService;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes a get globalconfiguration styles list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return await _globalConfigurationStylesService.GetGlobalConfigurationStyles();

			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a Update globalconfiguration styles
		/// </summary>
		/// <param name="globalConfigurationStyles"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update([FromBody] List<GlobalConfigurationStyle> globalConfigurationStyles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationStylesService.UpdateGlobalConfigurationStyles(globalConfigurationStyles);
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
