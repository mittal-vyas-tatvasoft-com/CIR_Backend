using CIR.Core.Entities;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
	public interface IRolesService
	{
		Task<List<Roles>> GetAllRoles();
		Task<Roles> GetRoleById(long roleid);

		Task<Boolean> RoleExists(string rolename);
		Task<IActionResult> CreateRole(RolesModel roles);
		Task<IActionResult> UpdateRole(Roles rolesModel);
		Task<IActionResult> DeleteRole(long roleid);
	}
}
