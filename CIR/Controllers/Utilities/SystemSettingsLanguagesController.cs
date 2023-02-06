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
	public class SystemSettingsLanguagesController
	{
		#region PROPERTIES
		private readonly ISystemSettingsLanguagesServices _isystemSettingsLanguagesServices;
		#endregion

		#region CONSTRUCTOR
		public SystemSettingsLanguagesController(ISystemSettingsLanguagesServices systemSettingsLanguagesServices)
		{
			_isystemSettingsLanguagesServices = systemSettingsLanguagesServices;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method takes Language details as parameters and Update Selected Languages 
		/// </summary>
		/// <param name="culture"> this object contains different parameters as details of a Languages </param>
		/// <returns ></returns>
		[HttpPut("[action]")]
		[CustomPermissionFilter(RolePrivilegesEnum.Language_Change)]
		public async Task<IActionResult> Update([FromBody] List<CulturesModel> culture)
		{
			try
			{
				return await _isystemSettingsLanguagesServices.UpdateSystemSettingsLanguage(culture);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		#endregion
	}
}
