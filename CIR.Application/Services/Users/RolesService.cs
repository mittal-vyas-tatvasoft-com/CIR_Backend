using CIR.Core.Entities.User;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.User;
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
		public async Task<RolesModel> GetAllRoles(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
		{
			return await _rolesRepository.GetAllUser(displayLength, displayStart, sortCol, search, sortAscending);
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
		public Task<IActionResult> CreateOrUpdateRoles(Roles roles)
		{
			return _rolesRepository.CreateOrUpdateRoles(roles);
		}
		public async Task<IActionResult> DeleteRole(long roleid)
		{
			return await _rolesRepository.DeleteRole(roleid);
		}


	}
}
