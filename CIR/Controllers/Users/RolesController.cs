using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Users
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class RolesController : ControllerBase
	{
		#region PROPETRTIES 
		private readonly IRolesService _rolesService;
		private readonly ILogger<RolesController> _logger;

		#endregion

		#region CONSTRUCTOR
		public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
		{
			_rolesService = rolesService;
			_logger = logger;
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method retuns filtered roles list using SP
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of roles </returns>
		[HttpGet]
		public async Task<IActionResult> GetAll(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
		{
			try
			{
				search ??= string.Empty;

				var rolesModel = await _rolesService.GetAllRoles(displayLength, displayStart, sortCol, search, sortAscending);

				if (rolesModel.RolesList.Count > 0)
				{
					return new JsonResult(new CustomResponse<RolesModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = rolesModel });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested roles were not found" });
			}
			catch (Exception ex)
			{

				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
			}
		}

		/// <summary>
		/// This method fetches single role data using role's Id
		/// </summary>
		/// <param name="id">role will be fetched according to this 'id'</param>
		/// <returns> role </returns> 
		[HttpGet("{id}")]
		public async Task<CustomResponse<Roles>> GetById([FromRoute] int id)
		{
			try
			{
				var roleslist = await _rolesService.GetRoleById(id);
				if (roleslist == null)
				{
					return new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound };
				}
				else
				{
					return new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = roleslist };
				}
			}
			catch (Exception ex)
			{
				return new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = true, Message = HttpStatusCodesMessages.InternalServerError };
			}
		}
		/// <summary>
		/// This method takes roles details as parameters and creates role and returns that role
		/// </summary>
		/// <param name="roles"> this object contains different parameters as details of a user </param>
		/// <returns > created role </returns>

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Roles roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var isExist = await _rolesService.RoleExists(roles.Name);
					if (isExist)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Role already exists" });
					}

					var newrole = await _rolesService.CreateOrUpdateRoles(roles);
					if (newrole != null)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Role Created" });
					}
					else
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Something went wrong" });
					}
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
		}

		/// <summary>
		/// This method takes roles details and updates the roles
		/// </summary>
		/// <param name="roles"> this object contains different parameters as details of a role </param>
		/// <returns> updated role </returns>
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] Roles roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _rolesService.CreateOrUpdateRoles(roles);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}

		/// <summary>
		/// This method disables role
		/// </summary>
		/// <param name="id"> role will be disabled according to this id </param>
		/// <returns> disabled role </returns>
		[HttpDelete("{roleid}")]
		public async Task<IActionResult> Delete(int roleid)
		{
			try
			{
				if (roleid > 0)
				{
					return await _rolesService.DeleteRole(roleid);
				}
				return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = false, Message = HttpStatusCodesMessages.NoContent, Data = "Invalid input id" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = ex });
			}
		}
		#endregion
	}
}
