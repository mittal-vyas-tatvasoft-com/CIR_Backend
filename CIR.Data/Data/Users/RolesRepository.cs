using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.GlobalConfiguration;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Roles = CIR.Core.Entities.Users.Roles;

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
		/// This method is used by get globalconfiguration fonts list
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> GetRoles()
		{
			try
			{
				var roles = await _CIRDbContext.Roles.Select(x => new RoleViewModel()
				{
					Id = x.Id,
					Description = x.Description,
					Name = x.Name
				}).ToListAsync();

				if (roles.Count == 0)
				{
					return new JsonResult(new CustomResponse<List<RoleViewModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
				return new JsonResult(new CustomResponse<List<RoleViewModel>>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = roles });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.InternalServerError, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.InternalServerError.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method retuns filtered role list using LINQ
		/// </summary>
		/// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
		/// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
		/// <param name="sortCol"> name of column which we want to sort</param>
		/// <param name="search"> word that we want to search in user table </param>
		/// <param name="sortAscending"> 'asc' or 'desc' direction for sort </param>
		/// <returns> filtered list of roles </returns>
		public async Task<RolesModel> GetAllRoles(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
		{
			RolesModel roles = new();
			IQueryable<Core.Entities.Users.Roles> roleList = roles.RolesList.AsQueryable();

			if (string.IsNullOrEmpty(sortCol))
			{
				sortCol = "Id";
			}

			try
			{
				roles.Count = _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).Count();

				roleList = sortAscending ? _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).OrderBy(x => EF.Property<object>(x, sortCol)).AsQueryable()
									 : _CIRDbContext.Roles.Where(y => y.Name.Contains(search) || y.Description.Contains(search)).OrderByDescending(x => EF.Property<object>(x, sortCol)).AsQueryable();

				var sortedRoles = await roleList.Skip(displayStart).Take(displayLength).ToListAsync();
				roles.RolesList = sortedRoles;

				return roles;
			}
			catch
			{
				return roles;
			}

		}

		/// <summary>
		/// this meethod checks if role exists or not based on input role name
		/// </summary>
		/// <param name="roleName"></param>
		/// <param name="id"></param>
		/// <returns> if user exists true else false </returns>
		public async Task<Boolean> RoleExists(string roleName, long id)
		{
			Roles checkroleExist;
			if (id == 0)
			{
				checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync();
			}
			else
			{
				checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == roleName && x.Id != id).FirstOrDefaultAsync();
			}
			if (checkroleExist != null)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// This method takes a add or update role
		/// </summary>
		/// <param name="roles"></param>
		/// <returns></returns>
		public async Task<IActionResult> AddRole(RolePermissionModel roles)
		{
			try
			{
				if (roles.Name == "" || roles.Name == "string")
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgEnterValidData });
				}

				if (roles.Id == 0)
				{
					Roles newRole = new()
					{
						Name = roles.Name,
						AllPermissions = roles.AllPermissions,
						CreatedOn = DateTime.Now,
						Description = roles.Description
					};

					_CIRDbContext.Roles.Add(newRole);
					await _CIRDbContext.SaveChangesAsync();

					var roleDetails = _CIRDbContext.Roles.Where(c => c.Name == roles.Name).FirstOrDefault();
					if (!roleDetails.AllPermissions)
					{
						foreach (var role in roles.Roles)
						{
							RoleGrouping roleGrouping = new()
							{
								RoleId = roleDetails.Id
							};
							_CIRDbContext.RolesGroupings.Add(roleGrouping);
							await _CIRDbContext.SaveChangesAsync();

							var roleGroupingDetail = _CIRDbContext.RolesGroupings.Where(c => c.RoleId == roleDetails.Id).OrderByDescending(c => c.Id).FirstOrDefault();
							if (roleGroupingDetail != null)
							{
								foreach (var item in role.site)
								{
									RoleGrouping2SubSite roleGrouping2SubSite = new RoleGrouping2SubSite()
									{
										RoleGroupingId = roleGroupingDetail.Id,
										SubSiteId = item.SiteId
									};
									await _CIRDbContext.RoleGrouping2SubSites.AddAsync(roleGrouping2SubSite);
									await _CIRDbContext.SaveChangesAsync();
									foreach (var items in item.Languages)
									{
										RoleGrouping2Culture roleGrouping2Culture = new RoleGrouping2Culture()
										{
											RoleGroupingId = roleGroupingDetail.Id,
											CultureLcid = items.CultureId
										};
										if (!_CIRDbContext.RoleGrouping2Cultures.Any(c => c.RoleGroupingId == roleGrouping2Culture.RoleGroupingId && c.CultureLcid == roleGrouping2Culture.CultureLcid))
										{
											await _CIRDbContext.RoleGrouping2Cultures.AddAsync(roleGrouping2Culture);
											await _CIRDbContext.SaveChangesAsync();
										}

										foreach (var subitem in items.Privileges)
										{

											RoleGrouping2Permission roleGrouping2Permission = new RoleGrouping2Permission()
											{
												RoleGroupingId = roleGroupingDetail.Id,
												PermissionEnumId = subitem.PrivilegesId
											};
											if (!_CIRDbContext.RoleGrouping2Permissions.Any(c => c.RoleGroupingId == roleGrouping2Permission.RoleGroupingId && c.PermissionEnumId == roleGrouping2Permission.PermissionEnumId))
											{
												await _CIRDbContext.RoleGrouping2Permissions.AddAsync(roleGrouping2Permission);
												await _CIRDbContext.SaveChangesAsync();
											}
										}
									}
								}
							}
						}
					}
					if (roles.Name != null)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Role") });
					}
				}
				else
				{

                    var rolesDetails = from var in _CIRDbContext.Roles
                                       where var.Id == roles.Id
                                       select var.CreatedOn;

					Roles updaterole = new()
					{
						Id = roles.Id,
						Name = roles.Name,
						AllPermissions = roles.AllPermissions,
						CreatedOn = rolesDetails.FirstOrDefault(),
						LastEditedOn = DateTime.Now,
						Description = roles.Description
					};
					_CIRDbContext.Entry(updaterole).State = EntityState.Modified;
					await _CIRDbContext.SaveChangesAsync();

					var roleDetails = _CIRDbContext.Roles.Where(c => c.Id == roles.Id).FirstOrDefault();
					if (!roleDetails.AllPermissions)
					{
						foreach (var role in roles.Roles)
						{
							if (role.groupId != 0)
							{
								var roleGroupingDetail = _CIRDbContext.RolesGroupings.Where(c => c.Id == role.groupId && c.RoleId == roleDetails.Id).FirstOrDefault();
								if (roleGroupingDetail != null)
								{
									_CIRDbContext.RoleGrouping2SubSites.RemoveRange(_CIRDbContext.RoleGrouping2SubSites.Where(x => x.RoleGroupingId == roleGroupingDetail.Id));
									await _CIRDbContext.SaveChangesAsync();
									_CIRDbContext.RoleGrouping2Cultures.RemoveRange(_CIRDbContext.RoleGrouping2Cultures.Where(x => x.RoleGroupingId == roleGroupingDetail.Id));
									await _CIRDbContext.SaveChangesAsync();
									_CIRDbContext.RoleGrouping2Permissions.RemoveRange(_CIRDbContext.RoleGrouping2Permissions.Where(x => x.RoleGroupingId == roleGroupingDetail.Id));
									await _CIRDbContext.SaveChangesAsync();

									foreach (var item in role.site)
									{

										RoleGrouping2SubSite roleGrouping2SubSite = new RoleGrouping2SubSite()
										{
											RoleGroupingId = roleGroupingDetail.Id,
											SubSiteId = item.SiteId
										};
										_CIRDbContext.RoleGrouping2SubSites.Add(roleGrouping2SubSite);
										await _CIRDbContext.SaveChangesAsync();
										foreach (var items in item.Languages)
										{
											RoleGrouping2Culture roleGrouping2Culture = new RoleGrouping2Culture()
											{
												RoleGroupingId = roleGroupingDetail.Id,
												CultureLcid = items.CultureId
											};
											if (!_CIRDbContext.RoleGrouping2Cultures.Any(c => c.RoleGroupingId == roleGrouping2Culture.RoleGroupingId && c.CultureLcid == roleGrouping2Culture.CultureLcid))
											{
												await _CIRDbContext.RoleGrouping2Cultures.AddAsync(roleGrouping2Culture);
												await _CIRDbContext.SaveChangesAsync();
											}

											foreach (var subitem in items.Privileges)
											{

												RoleGrouping2Permission roleGrouping2Permission = new RoleGrouping2Permission()
												{
													RoleGroupingId = roleGroupingDetail.Id,
													PermissionEnumId = subitem.PrivilegesId
												};
												if (!_CIRDbContext.RoleGrouping2Permissions.Any(c => c.RoleGroupingId == roleGrouping2Permission.RoleGroupingId && c.PermissionEnumId == roleGrouping2Permission.PermissionEnumId))
												{
													_CIRDbContext.RoleGrouping2Permissions.Add(roleGrouping2Permission);
													await _CIRDbContext.SaveChangesAsync();
												}
											}
										}
									}
								}
							}
							else
							{
								RoleGrouping roleGrouping = new()
								{
									RoleId = roleDetails.Id
								};
								_CIRDbContext.RolesGroupings.Add(roleGrouping);
								await _CIRDbContext.SaveChangesAsync();

								var roleGroupingDetail = _CIRDbContext.RolesGroupings.Where(c => c.RoleId == roleDetails.Id).OrderByDescending(c => c.Id).FirstOrDefault();
								if (roleGroupingDetail != null)
								{
									foreach (var item in role.site)
									{
										RoleGrouping2SubSite roleGrouping2SubSite = new RoleGrouping2SubSite()
										{
											RoleGroupingId = roleGroupingDetail.Id,
											SubSiteId = item.SiteId
										};
										await _CIRDbContext.RoleGrouping2SubSites.AddAsync(roleGrouping2SubSite);
										await _CIRDbContext.SaveChangesAsync();
										foreach (var items in item.Languages)
										{
											RoleGrouping2Culture roleGrouping2Culture = new RoleGrouping2Culture()
											{
												RoleGroupingId = roleGroupingDetail.Id,
												CultureLcid = items.CultureId
											};
											if (!_CIRDbContext.RoleGrouping2Cultures.Any(c => c.RoleGroupingId == roleGrouping2Culture.RoleGroupingId && c.CultureLcid == roleGrouping2Culture.CultureLcid))
											{
												await _CIRDbContext.RoleGrouping2Cultures.AddAsync(roleGrouping2Culture);
												await _CIRDbContext.SaveChangesAsync();
											}

											foreach (var subitem in items.Privileges)
											{

												RoleGrouping2Permission roleGrouping2Permission = new RoleGrouping2Permission()
												{
													RoleGroupingId = roleGroupingDetail.Id,
													PermissionEnumId = subitem.PrivilegesId
												};
												if (!_CIRDbContext.RoleGrouping2Permissions.Any(c => c.RoleGroupingId == roleGrouping2Permission.RoleGroupingId && c.PermissionEnumId == roleGrouping2Permission.PermissionEnumId))
												{
													await _CIRDbContext.RoleGrouping2Permissions.AddAsync(roleGrouping2Permission);
													await _CIRDbContext.SaveChangesAsync();
												}
											}
										}
									}
								}
							}
						}
					}
					else
					{
						var roleGroupingData = _CIRDbContext.RolesGroupings.Where(r => r.RoleId == roleDetails.Id).ToList();
						foreach (var item in roleGroupingData)
						{
							_CIRDbContext.RolesGroupings.Remove(item);
							await _CIRDbContext.SaveChangesAsync();
						}
					}
					if (roles.Name != null)
					{
						return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Saved, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Saved.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataSavedSuccessfully, "Role") });
					}
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute() });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a delete role and role permission
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		public async Task<IActionResult> DeleteRoles(long roleId)
		{
			var role = new Roles() { Id = roleId };
			try
			{
				if (_CIRDbContext.Users.Any(x => x.RoleId == roleId))
				{
					return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.BadRequest, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.BadRequest.GetDescriptionAttribute(), Data = SystemMessages.msgCannotRemoveRecord });
				}
				else
				{
					_CIRDbContext.Roles.Remove(role);
					await _CIRDbContext.SaveChangesAsync();
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Deleted, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Role") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}
		/// <summary>
		/// This method takes a remove section role id wise
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public async Task<IActionResult> RemoveSection(long groupId)
		{
			try
			{
				var roleGroupingsData = _CIRDbContext.RolesGroupings.Where(x => x.Id == groupId).ToList();
				foreach (var item in roleGroupingsData)
				{
					_CIRDbContext.RolesGroupings.Remove(item);
					await _CIRDbContext.SaveChangesAsync();
				}
				return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Deleted, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Deleted.GetDescriptionAttribute(), Data = string.Format(SystemMessages.msgDataDeletedSuccessfully, "Section") });
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a get role detail by roleid
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		public async Task<IActionResult> GetRoleDetailById(long roleId)
		{
			try
			{
				RolePermissionModel rolePermissionModel = new RolePermissionModel();
				var dictionaryobj = new Dictionary<string, object>
				{
					{ "roleId", roleId }
				};

				DataTable roleDetailDatatable = SQLHelper.ExecuteSqlQueryWithParams("spGetRoleDetailByRoleId", dictionaryobj);
				if (roleDetailDatatable.Rows.Count > 0)
				{
					rolePermissionModel = new();
					var listData = SQLHelper.ConvertToGenericModelList<RolePermissionModel>(roleDetailDatatable);
					rolePermissionModel.Id = listData[0].Id;
					rolePermissionModel.Name = listData[0].Name;
					rolePermissionModel.Description = listData[0].Description;
					rolePermissionModel.AllPermissions = listData[0].AllPermissions;
					if (rolePermissionModel.AllPermissions == false)
					{
						rolePermissionModel.Roles = GetRolesListData(roleDetailDatatable);
					}
					else
					{
						rolePermissionModel.Roles = null;
					}
					if (rolePermissionModel == null)
					{
						return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
					}
					return new JsonResult(new CustomResponse<RolePermissionModel>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.Success, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.Success.GetDescriptionAttribute(), Data = rolePermissionModel });
				}
				else
				{
					return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.NotFound, Result = false, Message = HttpStatusCodesAndMessages.HttpStatus.NotFound.GetDescriptionAttribute() });
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity, Result = true, Message = HttpStatusCodesAndMessages.HttpStatus.UnprocessableEntity.GetDescriptionAttribute(), Data = ex });
			}
		}

		/// <summary>
		/// This method takes a get role list data 
		/// </summary>
		/// <param name="roleListDatatable"></param>
		/// <returns></returns>
		public List<SubRolesModel> GetRolesListData(DataTable roleListDatatable)
		{
			List<SubRolesModel> subRoleList = new List<SubRolesModel>();

            var listMain = SQLHelper.ConvertToGenericModelList<SubModel>(roleListDatatable);
            var groupData = listMain.GroupBy(x => x.GroupId);
            foreach (var item in groupData)
            {
                var siteGroup = listMain.Where(x => x.GroupId == item.Key).GroupBy(x => x.SiteId);
                SubRolesModel subRole = new SubRolesModel
                {
                    site = new List<RoleGrouping2SubSiteModel>()
                };
                foreach (var itemSite in siteGroup)
                {
                    RoleGrouping2SubSiteModel roleGrouping2SubSiteModel = new RoleGrouping2SubSiteModel
                    {
                        SiteId = itemSite.Key,
                        Languages = new List<RoleGrouping2CultureModel>()
                    };

                    var cultureGroup = listMain.Where(x => x.GroupId == item.Key && x.SiteId == itemSite.Key).GroupBy(x => x.CultureId);
                    foreach (var itemCulture in cultureGroup)
                    {
                        RoleGrouping2CultureModel roleGrouping2CultureModel = new RoleGrouping2CultureModel
                        {
                            CultureId = itemCulture.Key,
                            Privileges = new List<RoleGrouping2PermissionModel>()
                        };

                        var privilegesGroup = listMain.Where(x => x.GroupId == item.Key && x.SiteId == itemSite.Key && x.CultureId == itemCulture.Key).GroupBy(x => x.PrivilegesId);
                        foreach (var itemprivileges in privilegesGroup)
                        {
                            RoleGrouping2PermissionModel roleGrouping2PermissionModel = new RoleGrouping2PermissionModel
                            {
                                PrivilegesId = itemprivileges.Key
                            };
                            roleGrouping2CultureModel.Privileges.Add(roleGrouping2PermissionModel);
                        }
                        roleGrouping2SubSiteModel.Languages.Add(roleGrouping2CultureModel);
                    }
                    subRole.site.Add(roleGrouping2SubSiteModel);
                }
                subRole.groupId = listMain.Where(x => x.GroupId == item.Key).FirstOrDefault().GroupId;
                subRoleList.Add(subRole);
            }
            return subRoleList;
        }
        #endregion
    }
}
