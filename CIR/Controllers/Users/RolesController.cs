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
		private readonly IRolesService _rolesService;
		public RolesController(IRolesService rolesService)
		{
			_rolesService = rolesService;
		}

		[HttpGet]
		public async Task<CustomResponse<List<Roles>>> GetAll()
		{
			try
			{
				var roleslist = await _rolesService.GetAllRoles();
				return new CustomResponse<List<Roles>>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = roleslist };
			}
			catch (Exception ex)
			{
				return new CustomResponse<List<Roles>>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = true, Message = HttpStatusCodesMessages.BadRequest };
			}
		}

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
				return new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = true, Message = HttpStatusCodesMessages.BadRequest };
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RolesModel roles)
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

					var newrole = await _rolesService.CreateRole(roles);
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
		[HttpPut]
		public async Task<IActionResult> Update([FromBody] Roles roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					return await _rolesService.UpdateRole(roles);
				}
				catch (Exception ex)
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
				}
			}
			return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
		}
		[HttpDelete("{roleid}")]
		public async Task<IActionResult> Delete(int roleid)
		{
			try
			{
				if (roleid > 0)
				{
					return await _rolesService.DeleteRole(roleid);
				}
				return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = ex });
			}
		}
	}
}
