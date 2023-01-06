using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
				if( roleslist == null)
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
		public async Task<CustomResponse<long>> Post([FromBody] RolesModel roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var newrole = await _rolesService.CreateRole(roles);
					if (newrole != null)
					{
						return new CustomResponse<long>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = newrole };
					}
					else
					{
						return new CustomResponse<long>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = true, Message = HttpStatusCodesMessages.BadRequest };
					}
				}
				catch (Exception ex)
				{
					return new CustomResponse<long>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = true, Message = HttpStatusCodesMessages.BadRequest };
				}
			}
			return new CustomResponse<long>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = true, Message = HttpStatusCodesMessages.BadRequest };
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int roleid)
		{
			if (roleid > 0)
			{
				return await _rolesService.DeleteRole(roleid);
			}
			return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
		}
	}
}
