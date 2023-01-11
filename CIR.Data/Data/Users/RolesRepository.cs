using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
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
        /// This method retuns filtered role list using LINQ
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
        /// <returns> filtered list of roles </returns>
        public async Task<RolesModel> GetAllRoles(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
        {
            RolesModel roles = new();
            IQueryable<Core.Entities.Users.Roles> temp = roles.RolesList.AsQueryable();

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
        /// this meethod checks if role exists or not based on input role name
        /// </summary>
        /// <param name="name"></param>
        /// <returns> if user exists true else false </returns>
        public async Task<Boolean> RoleExists(string rolename, long id)
        {
            Roles checkroleExist = new Roles();
            if (id == 0)
            {
                checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == rolename).FirstOrDefaultAsync();
            }
            else
            {
                checkroleExist = await _CIRDbContext.Roles.Where(x => x.Name == rolename && x.Id != id).FirstOrDefaultAsync();
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
                if (roles.Id == 0)
                {
                    Core.Entities.Users.Roles newrole = new()
                    {
                        Name = roles.Name,
                        AllPermissions = roles.AllPermissions,
                        CreatedOn = DateTime.Now,
                        Description = roles.Description
                    };

                    _CIRDbContext.Roles.Add(newrole);
                    await _CIRDbContext.SaveChangesAsync();

                    var roleDetails = _CIRDbContext.Roles.Where(c => c.Name == roles.Name).FirstOrDefault();
                    if (roleDetails != null)
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
                                        await _CIRDbContext.RoleGrouping2Cultures.AddAsync(roleGrouping2Culture);
                                        await _CIRDbContext.SaveChangesAsync();

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
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Role Saved Successfully." });
                    }
                }
                else
                {

                    var rolesDetails = (from var in _CIRDbContext.Roles
                                        where var.Id == roles.Id
                                        select var.CreatedOn);

                    Core.Entities.Users.Roles updaterole = new()
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
                    if (roleDetails != null)
                    {
                        foreach (var role in roles.Roles)
                        {
                            if (role.groupId != 0)
                            {
                                RoleGrouping roleGrouping = new()
                                {
                                    Id = role.groupId,
                                    RoleId = roleDetails.Id
                                };
                                _CIRDbContext.RolesGroupings.Update(roleGrouping);
                                await _CIRDbContext.SaveChangesAsync();

                                var roleGroupingDetail = _CIRDbContext.RolesGroupings.Where(c => c.Id == role.groupId && c.RoleId == roleDetails.Id).FirstOrDefault();
                                if (roleGroupingDetail != null)
                                {
                                    foreach (var item in role.site)
                                    {
                                        RoleGrouping2SubSite roleGrouping2SubSite = new RoleGrouping2SubSite()
                                        {
                                            RoleGroupingId = roleGroupingDetail.Id,
                                            SubSiteId = item.SiteId
                                        };
                                        _CIRDbContext.RoleGrouping2SubSites.Update(roleGrouping2SubSite);
                                        await _CIRDbContext.SaveChangesAsync();
                                        foreach (var items in item.Languages)
                                        {
                                            RoleGrouping2Culture roleGrouping2Culture = new RoleGrouping2Culture()
                                            {
                                                RoleGroupingId = roleGroupingDetail.Id,
                                                CultureLcid = items.CultureId
                                            };
                                            _CIRDbContext.RoleGrouping2Cultures.Update(roleGrouping2Culture);
                                            await _CIRDbContext.SaveChangesAsync();

                                            foreach (var subitem in items.Privileges)
                                            {

                                                RoleGrouping2Permission roleGrouping2Permission = new RoleGrouping2Permission()
                                                {
                                                    RoleGroupingId = roleGroupingDetail.Id,
                                                    PermissionEnumId = subitem.PrivilegesId
                                                };
                                                if (!_CIRDbContext.RoleGrouping2Permissions.Any(c => c.RoleGroupingId == roleGrouping2Permission.RoleGroupingId && c.PermissionEnumId == roleGrouping2Permission.PermissionEnumId))
                                                {
                                                    _CIRDbContext.RoleGrouping2Permissions.Update(roleGrouping2Permission);
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
                                            await _CIRDbContext.RoleGrouping2Cultures.AddAsync(roleGrouping2Culture);
                                            await _CIRDbContext.SaveChangesAsync();

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
                    if (roles.Name != null)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Role Updated Successfully." });
                    }
                }
                return new JsonResult(new CustomResponse<Core.Entities.Users.Roles>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }

        /// <summary>
        /// This method takes a delete role and role permission
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteRoles(long roleId)
        {
            var role = new Core.Entities.Users.Roles() { Id = roleId };
            try
            {
                _CIRDbContext.Roles.Remove(role);
                await _CIRDbContext.SaveChangesAsync();
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Role Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }
        /// <summary>
        /// This method takes a remove section role id wise
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IActionResult> RemoveSection(long groupId)
        {
            try
            {
                var data = _CIRDbContext.RolesGroupings.Where(x => x.Id == groupId).ToList();
                foreach (var item in data)
                {
                    _CIRDbContext.RolesGroupings.Remove(item);
                    await _CIRDbContext.SaveChangesAsync();
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = "Section Deleted Successfully." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }

        /// <summary>
        /// This method takes a get role detail by roleid
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<RolePermissionModel> GetRoleDetailById(long roleId)
        {
            RolePermissionModel rolePermissionModel = new RolePermissionModel();

            var dictionaryobj = new Dictionary<string, object>
            {
                { "roleId", roleId }
            };

            DataTable dt = SQLHelper.ExecuteSqlQueryWithParams("spGetRoleDetailByRoleId", dictionaryobj);

            rolePermissionModel = new();
            rolePermissionModel.Id = Convert.ToInt64(dt.Rows[0]["Id"].ToString());
            rolePermissionModel.Name = Convert.ToString(dt.Rows[1]["Name"].ToString());
            rolePermissionModel.Description = Convert.ToString(dt.Rows[2]["Description"].ToString());
            rolePermissionModel.AllPermissions = Convert.ToBoolean(dt.Rows[3]["AllPermissions"].ToString());
            rolePermissionModel.Roles = GetRolesListData(dt);
            return rolePermissionModel;
        }

        /// <summary>
        /// This method takes a get role list data 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<SubRolesModel> GetRolesListData(DataTable dt)
        {
            List<SubRolesModel> subRoleList = new List<SubRolesModel>();

            var listMain = SQLHelper.ConvertToGenericModelList<SubModel>(dt);
            var siteGroulp = listMain.GroupBy(x => x.SiteId);
            foreach (var itemSite in siteGroulp)
            {
                SubRolesModel subRole = new SubRolesModel();
                subRole.site = new List<RoleGrouping2SubSiteModel>();
                RoleGrouping2SubSiteModel roleGrouping2SubSiteModel = new RoleGrouping2SubSiteModel();
                roleGrouping2SubSiteModel.SiteId = itemSite.Key;
                roleGrouping2SubSiteModel.Languages = new List<RoleGrouping2CultureModel>();

                var cultureGroup = listMain.Where(x => x.SiteId == itemSite.Key).GroupBy(x => x.CultureId);
                foreach (var itemCulture in cultureGroup)
                {
                    RoleGrouping2CultureModel roleGrouping2CultureModel = new RoleGrouping2CultureModel();
                    roleGrouping2CultureModel.CultureId = itemCulture.Key;
                    roleGrouping2CultureModel.Privileges = new List<RoleGrouping2PermissionModel>();

                    var privilegesGroup = listMain.Where(x => x.SiteId == itemSite.Key && x.PrivilegesId == itemCulture.Key).GroupBy(x => x.CultureId);
                    foreach (var itemprivileges in privilegesGroup)
                    {
                        RoleGrouping2PermissionModel roleGrouping2PermissionModel = new RoleGrouping2PermissionModel();
                        roleGrouping2PermissionModel.PrivilegesId = itemprivileges.Key;
                        roleGrouping2CultureModel.Privileges.Add(roleGrouping2PermissionModel);
                    }
                    roleGrouping2SubSiteModel.Languages.Add(roleGrouping2CultureModel);
                }
                subRole.groupId = listMain.Where(x => x.SiteId == itemSite.Key).FirstOrDefault().GroupId;
                subRole.site.Add(roleGrouping2SubSiteModel);
                subRoleList.Add(subRole);
            }
            return subRoleList;
        }
        #endregion
    }
}
