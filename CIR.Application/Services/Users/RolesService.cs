using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Users
{
	public class RolesService : IRolesService
	{
		private readonly IRolesRepository _rolesRepository;

		public RolesService(IRolesRepository rolesRepository)
		{
			_rolesRepository = rolesRepository;
		}

		public async Task<List<Roles>> GetAllRoles()
		{
			var rolelist = _rolesRepository.GetAllRoles();
			return await rolelist;
		}

		public async Task<Roles> GetRoleById(long roleid)
		{
			var roles = await _rolesRepository.GetRoleById(roleid);
			return roles;
		}
		public async Task<Boolean> RoleExists(string rolename)
		{
			return await _rolesRepository.RoleExists(rolename);
		}
		public async Task<IActionResult> CreateRole(RolesModel roles)
		{
			return await _rolesRepository.CreateRole(roles);
		}

		public async Task<IActionResult> UpdateRole(Roles role)
		{
			return await _rolesRepository.UpdateRole(role);
		}
		public async Task<IActionResult> DeleteRole(long roleid)
		{
			return await _rolesRepository.DeleteRole(roleid);
		}


	}
}
