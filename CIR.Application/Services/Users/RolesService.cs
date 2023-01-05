using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public async Task<long> CreateRole(RolesModel roles)
		{
			return await _rolesRepository.CreateRole(roles);
		}

		public async Task UpdateRole(RolesModel rolesModel)
		{
			 await _rolesRepository.UpdateRole(rolesModel);
		}
		public async Task<IActionResult> DeleteRole(long roleid)
		{
			return await _rolesRepository.DeleteRole(roleid);
		}


	}
}
