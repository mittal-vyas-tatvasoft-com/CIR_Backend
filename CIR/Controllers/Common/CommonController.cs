using CIR.Common.CommonModels;
using CIR.Common.CustomResponse;
using CIR.Core.Entities.Utilities;
using CIR.Core.Interfaces.Common;
using CIR.Core.ViewModel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Common
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CommonController : ControllerBase
	{
		#region PROPERTIES

		private readonly ICommonService _commonService;

		#endregion

		#region CONSTRUCTORS

		public CommonController(ICommonService commonService)
		{
			_commonService = commonService;
		}

		#endregion

		#region METHODS

		/// <summary>
		/// This method get a currencies list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetCurrencies")]
		public async Task<IActionResult> GetCurrencies()
		{
			try
			{
				return await _commonService.GetCurrencies();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });

			}

		}

		/// <summary>
		/// This method get a country list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetCountries")]
		public async Task<IActionResult> GetCountries()
		{
			try
			{
				return await _commonService.GetCountries();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });

			}
		}

		/// <summary>
		/// This method get a culture list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetCultures")]
		public async Task<IActionResult> GetCultures()
		{
			try
			{
				return await _commonService.GetCultures();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method get a site or section list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetSubSites")]
		public async Task<IActionResult> GetSubSites()
		{
			try
			{
				return await _commonService.GetSubSites();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method get a role privileges list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetRolePrivileges")]
		public async Task<IActionResult> GetRolePrivileges()
		{
			try
			{
				return await _commonService.GetRolePrivileges();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method get a Salutationtype list
		/// <para>code</para>
		/// </summary>
		/// <returns></returns>
		[HttpGet("[action]")]
		public async Task<IActionResult> GetSalutationTypeList(string code)
		{
			try
			{
				return await _commonService.GetSalutationTypeList(code);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = true, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method get a SystemCode list
		/// </summary>
		/// <returns></returns>
		[HttpGet("[action]")]
		public async Task<IActionResult> GetSystemCodes()
		{
			try
			{
				return await _commonService.GetSystemCodes();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = true, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		#endregion
	}
}
