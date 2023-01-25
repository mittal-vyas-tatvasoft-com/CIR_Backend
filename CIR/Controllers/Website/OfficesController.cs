using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Websites;
using CIR.Core.Interfaces.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Website
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OfficesController : ControllerBase
	{
		#region PROPERTIES
		private readonly IOfficeService _officeService;
		#endregion

        #region CONSTRUCTOR
        public OfficesController(IOfficeService OfficeService)
        {
            _officeService = OfficeService;
        }
        #endregion

		#region METHODS
		/// <summary>
		/// This method retuns filtered holidays list using SP
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of holidays </returns>
		[HttpGet]
		public async Task<IActionResult> GetAllOffices(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
		{
			try
			{
				search ??= string.Empty;
				return await _officeService.GetOffices(displayLength, displayStart, sortCol, search, sortAscending);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method fetches single office data using office's Id
		/// </summary>
		/// <param name="id">user will be fetched according to this 'id'</param>
		/// <returns> user </returns> 
		/// 
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOfficeById(long id)
		{
			try
			{
				return await _officeService.GetOfficesById(id);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes office details as parameters and creates user and returns that office
		/// </summary>
		/// <param name="office"> this object contains different parameters as details of a office </param>
		/// <returns > created office </returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create([FromBody] Offices office)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _officeService.CreateOrUpdateOffice(office);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method takes office details and updates the office
		/// </summary>
		/// <param name="office"> this object contains different parameters as details of a office </param>
		/// <returns> updated office </returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update([FromBody] Offices office)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _officeService.CreateOrUpdateOffice(office);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgBadRequest });
		}

		/// <summary>
		/// This method disables office 
		/// </summary>
		/// <param name="id"> office will be disabled according to this id </param>
		/// <returns> disabled office</returns>
		[HttpDelete("[action]")]
		public async Task<IActionResult> Delete(long id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (id > 0)
					{
						return await _officeService.DeleteOffice(id);
					}
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidId });
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
