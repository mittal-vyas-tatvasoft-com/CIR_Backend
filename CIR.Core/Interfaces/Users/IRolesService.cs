using CIR.Core.Entities;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
	public interface IRolesService
	{
		Task<RolesModel> GetAllRoles(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true);
		Task<Roles> GetRoleById(long roleid);
		Task<Boolean> RoleExists(string rolename);
		Task<IActionResult> CreateOrUpdateRoles(Roles roles);
		Task<IActionResult> DeleteRole(long roleid);
	}
}
