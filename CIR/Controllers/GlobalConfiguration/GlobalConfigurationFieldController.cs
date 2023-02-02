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
	public class GlobalConfigurationFieldController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationFieldServices _globalConfigurationFieldServices;
		#endregion

		#region CONSTRUCTOR
		public GlobalConfigurationFieldController(IGlobalConfigurationFieldServices globalConfigurationFieldServices)
		{
			_globalConfigurationFieldServices = globalConfigurationFieldServices;
		}
		#endregion

		#region METHODS
		/// <summary>
		/// This method returns list of global config fields 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetAllGlobalConfigurationFields()
		{
			try
			{
				return await _globalConfigurationFieldServices.GetAllFields();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create globalconfiguration reason
		/// </summary>
		/// <param name="globalConfigurationFields"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<GlobalConfigurationField> globalConfigurationFields)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationFieldServices.CreateOrUpdateFields(globalConfigurationFields);
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
		public async Task<IActionResult> Update(List<GlobalConfigurationField> globalConfigurationFields)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationFieldServices.CreateOrUpdateFields(globalConfigurationFields);
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
