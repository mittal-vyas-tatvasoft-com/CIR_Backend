using CIR.Core.Entities;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Users
{
	public interface IRolesService
	{
		Task<List<Roles>> GetAllRoles();
		Task<Roles> GetRoleById(long roleid);
		Task<long> CreateRole(RolesModel roles);
		Task UpdateRole(RolesModel rolesModel);
		Task<IActionResult> DeleteRole(long roleid);
	}
}
