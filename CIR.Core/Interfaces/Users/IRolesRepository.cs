using CIR.Core.Entities.Users;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
	public interface IRolesRepository
	{
		Task<RolesModel> GetAllUser(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true);
		Task<Roles> GetRoleById(long roleid);
		Task<Boolean> RoleExists(string rolename);
		Task<IActionResult> DeleteRole(long roleid);
		Task<IActionResult> CreateOrUpdateRoles(Roles roles);
	}
}
