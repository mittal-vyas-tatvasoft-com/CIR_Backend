using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
	[Route("api/[controller]")]
	[ApiController]
	public class Portal2GlobalConfigurationStylesController : ControllerBase
	{
		#region PROPERTIES
		private readonly IPortal2GlobalConfigurationStylesService _portal2GlobalConfigurationStylesService;
		#endregion

		#region CONSTRUCTORS
		public Portal2GlobalConfigurationStylesController(IPortal2GlobalConfigurationStylesService portal2GlobalConfigurationStylesService)
		{
			_portal2GlobalConfigurationStylesService = portal2GlobalConfigurationStylesService;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes a get GetPortalToGlobalConfigurationStylesList
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		[HttpGet("{portalId}")]
		public async Task<IActionResult> GetPortalToGlobalConfigurationStylesList(long portalId)
		{
			try
			{
				return await _portal2GlobalConfigurationStylesService.GetPortalToGlobalConfigurationStylesList(portalId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a update portalToGlobalConfigrationStyles
		/// </summary>
		/// <param name="portal2GlobalConfigurationStyles"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<Portal2GlobalConfigurationStyle> portal2GlobalConfigurationStyles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _portal2GlobalConfigurationStylesService.UpdatePortalToGlobalConfigurationStyles(portal2GlobalConfigurationStyles);
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
