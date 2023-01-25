using CIR.Common.CustomResponse;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Usersvm;
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
		/// This method takes a get Roles list
		/// </summary>
		/// <returns></returns>
		[HttpGet("GetAllroles")]
		public async Task<IActionResult> GetRoles()
		{
			try
			{
				return await _rolesService.GetRoles();
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered roles list using SP
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of roles </returns>
		[HttpGet]
		public async Task<IActionResult> GetAllRoles(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
		{
			try
			{
				search ??= string.Empty;

				var rolesModel = await _rolesService.GetAllRoles(displayLength, displayStart, sortCol, search, sortAscending);

				if (rolesModel.RolesList.Count > 0)
				{
					return new JsonResult(new CustomResponse<RolesModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = rolesModel });
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgNotFound, "Roles") });

			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method return role detail of given id
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		[HttpGet("{roleId}")]
		public async Task<IActionResult> Get(int roleId)
		{
			try
			{
				return await _rolesService.GetRoleDetailById(roleId);
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes roles details and add role
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		[HttpPost("[action]")]
		public async Task<IActionResult> Create(RolePermissionModel roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var isExist = await _rolesService.RoleExists(roles.Name, roles.Id);
					if (isExist)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataExists, "Role") });
					}
					else
					{
						return await _rolesService.AddRole(roles);
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
		/// This method takes roles details and update role
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		[HttpPut("[action]")]
		public async Task<IActionResult> Update(RolePermissionModel roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var isExist = await _rolesService.RoleExists(roles.Name, roles.Id);
					if (isExist)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataExists, "Role") });
					}
					else
					{
						return await _rolesService.AddRole(roles);
					}
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataExists, "Role") });
		}

		/// <summary>
		/// This method takes roles details and delete role
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		[HttpDelete("[action]")]
		public async Task<IActionResult> Delete(long roleId)
		{
			try
			{
				if (roleId > 0)
				{
					return await _rolesService.DeleteRoles(roleId);
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidId });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes remove role id wise section
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		[HttpDelete("RemoveSection/{groupId}")]
		public async Task<IActionResult> RemoveSection(long groupId)
		{
			try
			{
				if (groupId > 0)
				{
					return await _rolesService.RemoveSection(groupId);
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute(), Data = SystemMessages.msgInvalidId });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}
		#endregion
	}
}
