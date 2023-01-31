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
	public class GlobalConfigurationEmailsController : ControllerBase
	{
		#region PROPERTIES
		private readonly IGlobalConfigurationEmailsService _globalConfigurationEmailsService;
		#endregion

		#region CONSTRUCTORS
		public GlobalConfigurationEmailsController(IGlobalConfigurationEmailsService globalConfigurationEmailsService)
		{
			_globalConfigurationEmailsService = globalConfigurationEmailsService;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes a update globalconfiguration Emails
		/// </summary>
		/// <param name="globalConfigurationEmails"></param>
		/// <returns></returns>
		[HttpPut]
		public async Task<IActionResult> Update(List<GlobalConfigurationEmails> globalConfigurationEmails)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationEmailsService.CreateOrUpdateGlobalConfigurationEmails(globalConfigurationEmails);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });

		}
		/// <summary>
		/// This method takes a get globalconfiguration email list
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{cultureId}")]
		public async Task<IActionResult> Get(int cultureId)
		{
			try
			{
				return await _globalConfigurationEmailsService.GetGlobalConfigurationEmailsDataList(cultureId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}