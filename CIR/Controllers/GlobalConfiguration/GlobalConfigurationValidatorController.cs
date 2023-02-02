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
	public class GlobalConfigurationValidatorController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationValidatorService _globalConfigurationValidatorService;

		#endregion
		#region CONSTRUCTOR
		public GlobalConfigurationValidatorController(IGlobalConfigurationValidatorService globalConfigurationValidatorService)
		{
			_globalConfigurationValidatorService = globalConfigurationValidatorService;
		}
		#endregion
		#region METHODS

		/// <summary>
		/// This method get globalconfiguration Validators list
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetGlobalConfigurationValidators()
		{
			try
			{
				return await _globalConfigurationValidatorService.GetGlobalConfigurationValidators();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}


		/// <summary>
		/// This method takes a create globalConfiguration Validators
		/// </summary>
		/// <param name="globalConfigurationValidators"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<GlobalConfigurationValidator> globalConfigurationValidators)
		{
			try
			{
				return await _globalConfigurationValidatorService.CreateOrUpdateGlobalConfigurationValidators(globalConfigurationValidators);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		/// <summary>
		/// This method takes a update globalconfiguration Validators
		/// </summary>
		/// <param name="globalConfigurationReasons"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<GlobalConfigurationValidator> globalConfigurationValidators)
		{
			try
			{
				return await _globalConfigurationValidatorService.CreateOrUpdateGlobalConfigurationValidators(globalConfigurationValidators);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
