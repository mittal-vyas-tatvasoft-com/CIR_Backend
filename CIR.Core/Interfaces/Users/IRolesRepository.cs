using CIR.Core.Entities;
using CIR.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Users
{
	public interface IRolesRepository
	{
		Task<List<Roles>> GetAllRoles();
		Task<Roles> GetRoleById(long roleid);
		Task<long> CreateRole(RolesModel rolemodel);
		Task UpdateRole(RolesModel rolesModel);
		Task<Roles> DeleteRole(long roleid);
	}
}
