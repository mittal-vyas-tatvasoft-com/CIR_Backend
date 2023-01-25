using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Interfaces.GlobalConfiguration;
using CIR.Core.ViewModel.GlobalConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.GlobalConfiguration
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class GlobalConfigurationCurrenciesController : ControllerBase
	{
		#region PROPERTIES

		private readonly IGlobalConfigurationCurrenciesService _globalConfigurationCurrenciesService;

		#endregion

		#region CONSTRUCTORS

		public GlobalConfigurationCurrenciesController(IGlobalConfigurationCurrenciesService globalConfigurationCurrenciesService)
		{
			_globalConfigurationCurrenciesService = globalConfigurationCurrenciesService;
		}

		#endregion

		#region METHODS

		/// <summary>
		/// This method takes get global currency country wise
		/// </summary>
		/// <param name="countryId">this object contains countryId</param>
		/// <returns></returns>
		[HttpGet("{countryId}")]
		public async Task<IActionResult> GetGlobalConfigurationCurrenciesCountryWise(int countryId)
		{
			try
			{
				return await _globalConfigurationCurrenciesService.GetGlobalConfigurationCurrenciesCountryWise(countryId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes add global currency
		/// </summary>
		/// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(List<GlobalCurrencyModel> globalCurrencyModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationCurrenciesService.CreateOrUpdateGlobalConfigurationCurrencies(globalCurrencyModel);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes update global currency
		/// </summary>
		/// <param name="globalCurrencyModel">this object contains different parameters as details of a globalcurrency</param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(List<GlobalCurrencyModel> globalCurrencyModel)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _globalConfigurationCurrenciesService.CreateOrUpdateGlobalConfigurationCurrencies(globalCurrencyModel);
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
