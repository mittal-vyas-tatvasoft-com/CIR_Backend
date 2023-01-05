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
			_rolesService= rolesService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var roleslist = await _rolesService.GetAllRoles();
				return Ok(roleslist);
			}
			catch (Exception ex) 
			{
				return BadRequest(new { message = "Error : " + ex });
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById([FromRoute] int id)
		{
			try
			{
				var roleslist = await _rolesService.GetRoleById(id);
				return Ok(roleslist);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Error : " + ex });
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] RolesModel  roles)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var newrole = await _rolesService.CreateRole(roles);
					if (newrole != null)
					{
						return Ok(newrole);
					}
					else
					{
						return BadRequest(new { message = "Error occured creating new role" });
					}
				}
				catch (Exception ex)
				{
					return BadRequest(new { message = "Error : " + ex + " Invalid input data" }); ;
				}
			}
				return BadRequest();
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int roleid)
		{
			if (roleid > 0)
			{
				var deletedrole = await _rolesService.DeleteRole(roleid);

				if (deletedrole.Id > 0 )
				{
					return Ok(deletedrole);
				}
				return BadRequest(new { message = "Invalid input" });
			}
			return BadRequest(new { message = "Invalid input" });
		}
	}
}
