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
	public class GlobalConfigurationReasonsController : ControllerBase
	{
		#region PROPERTIES

		private readonly IGlobalConfigurationReasonsService _globalConfigurationReasonsService;

		#endregion

		#region CONSTRUCTORS

		public GlobalConfigurationReasonsController(IGlobalConfigurationReasonsService globalConfigurationReasonsService)
		{
			_globalConfigurationReasonsService = globalConfigurationReasonsService;
		}

		#endregion


		#region METHODS

		/// <summary>
		/// This method takes a get globalconfiguration reasons list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetGlobalConfigurationReasons()
		{
			try
			{
				return await _globalConfigurationReasonsService.GetGlobalConfigurationReasons();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create globalconfiguration reason
		/// </summary>
		/// <param name="globalConfigurationReasons"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<GlobalConfigurationReasons> globalConfigurationReasons)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationReasonsService.CreateOrUpdateGlobalConfigurationReasons(globalConfigurationReasons);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}


		/// <summary>
		/// This method takes a update globalconfiguration reason
		/// </summary>
		/// <param name="globalConfigurationReasons"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<GlobalConfigurationReasons> globalConfigurationReasons)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationReasonsService.CreateOrUpdateGlobalConfigurationReasons(globalConfigurationReasons);
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
