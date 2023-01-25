using CIR.Common.CustomResponse;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.Utilities;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Utilities
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SystemSettingsLookupsController : ControllerBase
	{
		#region PROPERTIES
		private readonly ISystemSettingsLookupsService _lookupService;
		#endregion

		#region CONSTRUCTOR
		public SystemSettingsLookupsController(ISystemSettingsLookupsService lookupService)
		{
			_lookupService = lookupService;
		}
		#endregion

		#region METHODS
		[HttpGet("[action]")]
		/// <summary>
		/// This method takes Lookups details and updates the LookupItem
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="code"></param>
		/// <param name="sortCol"></param>
		/// <param name="searchCultureCode"></param>
		/// <param name="sortAscending"></param>
		/// <returns> get cultureCodeList </returns>
		public async Task<IActionResult> Get(long? cultureId, string? code, string? sortCol, string? searchCultureCode, bool sortAscending = true)
		{
			try
			{
				searchCultureCode ??= string.Empty;
				return await _lookupService.GetAllCultureCodeList(cultureId, code, sortCol, searchCultureCode, sortAscending);
			}
			catch (Exception ex)
			{

				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		[HttpGet("[action]")]
		/// <summary>
		/// This method takes Lookups details and updates the LookupItem
		/// </summary>
		/// <param name="cultureId"></param>
		/// <param name="code"></param>
		/// <param name="searchLookupItems"></param>
		/// <param name="sortAscending"></param>
		/// <returns> get LookupItemList </returns>
		public async Task<IActionResult> GetLookupItemList(long cultureId, string code, string? searchLookupItems, bool sortAscending = true)
		{
			try
			{
				searchLookupItems ??= string.Empty;
				return await _lookupService.GetAllLookupsItems(cultureId, code, searchLookupItems, sortAscending);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes LookupItemsText details and add LookupItemsText
		/// </summary>
		/// <param name="lookupItemsTextmodel"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(LookupItemsTextModel lookupItemsTextmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var isExist = await _lookupService.LookupItemExists(lookupItemsTextmodel.CultureId, lookupItemsTextmodel.LookupItemId);
					if (isExist)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataExists, "Lookup Item") });
					}
					else
					{
						return await _lookupService.CreateOrUpdateLookupItem(lookupItemsTextmodel);
					}
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes Lookups details and updates the LookupItem
		/// </summary>
		/// <param name="lookupItemsTextmodel"> this object contains different parameters as details of a lookupItem </param>
		/// <returns> updated LookupItems </returns>

		[HttpPut("[action]")]
		public async Task<IActionResult> Update(LookupItemsTextModel lookupItemsTextmodel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _lookupService.CreateOrUpdateLookupItem(lookupItemsTextmodel);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		[HttpGet("[action]/{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				return await _lookupService.GetLookupById(id);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		#endregion
	}
}
