using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CIR.Core.Interfaces.Users;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using CIR.Common.Data;
using CIR.Core.Entities;
using Microsoft.EntityFrameworkCore;
using CIR.Core.ViewModel;
using Azure;
using Microsoft.AspNetCore.Mvc;
using CIR.Common.CustomResponse;

namespace CIR.Data.Data.Users
{
	public class RolesRepository : ControllerBase, IRolesRepository
	{
		private readonly CIRDbContext _CIRDbContext;
		public RolesRepository(CIRDbContext context)
		{
			_CIRDbContext = context ??
				throw new ArgumentNullException(nameof(context));
		}

		public async Task<List<Roles>> GetAllRoles()
		{
			var roleslist = await _CIRDbContext.Roles.ToListAsync();
			return roleslist;
		}

		public async Task<Roles> GetRoleById(long roleid)
		{
			var role = await _CIRDbContext.Roles.FindAsync(roleid);
			return role;
		}

		public async Task<long> CreateRole(RolesModel rolemodel)
		{
			var role = new Roles()
			{
				Name = rolemodel.Name,
				Description = rolemodel.Description,
				AllPermissions = rolemodel.AllPermissions,
				CreatedOn = rolemodel.CreatedOn,
			};
			_CIRDbContext.Roles.Add(role);
			await _CIRDbContext.SaveChangesAsync();

			return role.Id;
		}

		public async Task UpdateRole(RolesModel rolesModel)
		{
			var role = new Roles()
			{
				Name = rolesModel.Name,
				AllPermissions = rolesModel.AllPermissions,
				Description = rolesModel.Description,
			};
			_CIRDbContext.Roles.Update(role);
			await _CIRDbContext.SaveChangesAsync();
		}

		public async Task<IActionResult> DeleteRole(long roleid)
		{
			var role = new Roles() { Id = roleid };
			try
			{
				_CIRDbContext.Roles.Remove(role);
				await _CIRDbContext.SaveChangesAsync();
				return Ok(new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = new Roles() });
			}
			catch (Exception ex)
			{
				return UnprocessableEntity(new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = new Roles() });
			}
		}
	}
}
