using CIR.Common.Data;
using CIR.Common.Enums;
using CIR.Core.Entities.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Security.Claims;

namespace CIR.Common.Helper
{
    public class CustomPermissionFilter : Attribute, IActionFilter
    {
        private RolePrivilegesEnum _rolePriviledgesEnums;
        public CustomPermissionFilter(RolePrivilegesEnum rolePriviledgesEnums)
        {
            _rolePriviledgesEnums = rolePriviledgesEnums;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //TODO when functionality start completely need to remove below return line
            return;
            ClaimsPrincipal claimsPrincipals = context.HttpContext.User;
            var lstAccessCode = claimsPrincipals.Claims.FirstOrDefault(x => x.Type == "RoleId").Value;
            var roleData = GetRoleDetailById(Convert.ToInt64(lstAccessCode));
            if (roleData.Result != null)
            {
                if (roleData.Result.AllPermissions)
                {
                    return;
                }
                else
                {
                    if (roleData.Result.Roles.Any(x => x.site.Any(x => x.Languages.Any(x => x.Privileges.Any(x => x.PrivilegesId == (int)_rolePriviledgesEnums)))))
                    {
                        return;
                    }
                    context.Result = new ForbidResult();
                    context.Result = new UnauthorizedObjectResult("You don’t have permission to access on this method");
                    return;
                }
            }
        }
        public async Task<RolePermissionModel> GetRoleDetailById(long roleId)
        {

            RolePermissionModel rolePermissionModel = new RolePermissionModel();
            var dictionaryobj = new Dictionary<string, object>
                {
                    { "roleId", roleId }
                };

            DataTable roleDetailDatatable = SQLHelper.ExecuteSqlQueryWithParams("spGetRoleDetailByRoleId", dictionaryobj);

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
            return rolePermissionModel;
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
    }
}
