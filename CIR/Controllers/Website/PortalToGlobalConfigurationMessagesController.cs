﻿using CIR.Common.CustomResponse;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Website;
using CIR.Core.Interfaces.Website;
using CIR.Core.Interfaces.Websites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PortalToGlobalConfigurationMessagesController : ControllerBase
	{
		#region PROPERTIES
		private readonly IPortalToGlobalConfigurationMessagesService _portalToGlobalConfigurationMessagesService;
		#endregion

		#region CONSTRUCTORS
		public PortalToGlobalConfigurationMessagesController(IPortalToGlobalConfigurationMessagesService portalToGlobalConfigurationMessagesService)
		{
			_portalToGlobalConfigurationMessagesService = portalToGlobalConfigurationMessagesService;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes a get GetPortalToGlobalConfigurationMessagesList
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		[HttpGet("{portalId}")]
		public async Task<IActionResult> GetPortalToGlobalConfigurationMessagesList(int portalId)
		{
			try
			{
				return await _portalToGlobalConfigurationMessagesService.GetPortalToGlobalConfigurationMessagesList(portalId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method takes a create portalToGlobalConfigration Messages
		/// </summary>
		/// <param name="portal2GlobalConfigurationMessages"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessages)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _portalToGlobalConfigurationMessagesService.CreateOrUpdatePortalToGlobalConfigurationMessages(portal2GlobalConfigurationMessages);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}

		/// <summary>
		/// This method takes a update portalToGlobalConfigration Messages
		/// </summary>
		/// <param name="portal2GlobalConfigurationMessages"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<Portal2GlobalConfigurationMessage> portal2GlobalConfigurationMessages)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _portalToGlobalConfigurationMessagesService.CreateOrUpdatePortalToGlobalConfigurationMessages(portal2GlobalConfigurationMessages);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}
		#endregion
	}
}
