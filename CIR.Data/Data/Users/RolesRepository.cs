using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		public async Task<Boolean> RoleExists(string rolename)
		{
			var checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == rolename).FirstOrDefaultAsync();

			if (checkroleExist != null && checkroleExist.Id > 0)
			{
				return true;
			}
			return false;
		}

		public async Task<IActionResult> CreateRole(RolesModel rolemodel)
		{
			try
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

				return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = "Role Inserted Successfully" }); ;
			}
			catch (Exception ex)
			{
				return UnprocessableEntity(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}

		}

		public async Task<IActionResult> UpdateRole(Roles rolesModel)
		{
			try
			{
				var role = new Roles()
				{
					Id = rolesModel.Id,
					CreatedOn = rolesModel.CreatedOn,
					Name = rolesModel.Name,
					AllPermissions = rolesModel.AllPermissions,
					Description = rolesModel.Description,
				};
				_CIRDbContext.Roles.Update(role);
				await _CIRDbContext.SaveChangesAsync();
				return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Role Updated Successfuly" });
			}
			catch (Exception ex)
			{
				return UnprocessableEntity(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}

		}

		public async Task<IActionResult> DeleteRole(long roleid)
		{
			var role = new Roles() { Id = roleid };
			try
			{
				_CIRDbContext.Roles.Remove(role);
				await _CIRDbContext.SaveChangesAsync();
				return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Role Deleted Successfully" });
			}
			catch (Exception ex)
			{
				return UnprocessableEntity(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
			}
		}
	}
}
