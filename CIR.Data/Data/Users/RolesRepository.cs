using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Core.Entities.User;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CIR.Data.Data.Users
{
    public class RolesRepository : ControllerBase, IRolesRepository
	{
		#region PROPERTIES
		private readonly CIRDbContext _CIRDbContext;
		#endregion

		#region CONSTRUCTOR
		public RolesRepository(CIRDbContext context)
		{
			_CIRDbContext = context ??
				throw new ArgumentNullException(nameof(context));
		}
		#endregion

		#region METHODS

		/// <summary>
		/// This method retuns filtered role list using LINQ
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of roles </returns>
		public async Task<RolesModel> GetAllUser(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
		{
			RolesModel roles = new();
			IQueryable<Roles> temp = roles.RolesList.AsQueryable();

			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}

			try
			{
				roles.Count = _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).Count();

				temp = sortAscending ? _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).OrderBy(x => EF.Property<object>(x, sortCol)).AsQueryable()
									 : _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).OrderByDescending(x => EF.Property<object>(x, sortCol)).AsQueryable();

				var sortedData = await temp.Skip(displayStart).Take(displayLength).ToListAsync();
				roles.RolesList = sortedData;

				return roles;
			}
			catch
			{
				return roles;
			}

		}

		/// <summary>
		/// fetches role based on input role id
		/// </summary>
		/// <param name="id"></param>
		/// <returns> role or null role if not found </returns>
		public async Task<Roles> GetRoleById(long roleid)
		{
			var role = await _CIRDbContext.Roles.FindAsync(roleid);
			return role;
		}
		/// <summary>
		/// this meethod checks if role exists or not based on input role name
		/// </summary>
		/// <param name="name"></param>
		/// <returns> if user exists true else false </returns>
		public async Task<Boolean> RoleExists(string rolename)
		{
			var checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == rolename).FirstOrDefaultAsync();

			if (checkroleExist != null && checkroleExist.Id > 0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// This method is used by create method and update method of role controller
		/// </summary>
		/// <param name="role"> new role data or update data for role </param>
		/// <returns> Ok status if its valid else unprocessable </returns>

		public async Task<IActionResult> CreateOrUpdateRoles(Roles roles)
		{
			Roles newrole = new()
			{
				Id = roles.Id,
				Name = roles.Name,
				AllPermissions = roles.AllPermissions,
				CreatedOn = roles.CreatedOn,
				Description = roles.Description,
			};
			if (roles.Id > 0)
			{
				_CIRDbContext.Roles.Update(newrole);
			}
			else
			{
				_CIRDbContext.Roles.Add(newrole);
			}

			await _CIRDbContext.SaveChangesAsync();
			if (roles.Name != null)
			{
				return Ok(new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
			}
			return UnprocessableEntity(new CustomResponse<Roles>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
		}

		/// <summary>
		/// this metohd updates a column value and disables role
		/// </summary>
		/// <param name="id"></param>
		/// <returns> deleted/disabled role </returns>
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
		#endregion
	}
}
