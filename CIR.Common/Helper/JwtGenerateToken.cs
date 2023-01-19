using CIR.Common.CommonModels;
using CIR.Core.Entities.Users;
using CIR.Core.ViewModel;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CIR.Common.Helper
{
    public class JwtGenerateToken
    {
        private readonly JwtAppSettings _jwtAppSettings;
        public JwtGenerateToken(IOptions<JwtAppSettings> jwtAppSettings)
        {
            _jwtAppSettings = jwtAppSettings.Value;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            string jwtToken = string.Empty;
            try
            {
                var roleDetail = GetRolesDetailByRoleId(user.RoleId);
                var rolesList = JsonConvert.SerializeObject(roleDetail.Result);
                // generate token that is valid for 20 minutes
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtAppSettings.AuthKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                        {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName),
                        new Claim("RoleId",roleDetail.Result.Id.ToString()),
                        new Claim("RoleName",roleDetail.Result.Name),
                        new Claim("AllPermissions",roleDetail.Result.AllPermissions.ToString()),
                        new Claim("Roles",rolesList)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                jwtToken = tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                throw;
            }
            return jwtToken;
        }
        public async Task<RolePermissionModel> GetRolesDetailByRoleId(long roleId)
        {
            RolePermissionModel rolePermissionModel = new RolePermissionModel();
            var dictionaryobj = new Dictionary<string, object>
                {
                    { "roleId", roleId }
                };

            DataTable dt = SQLHelper.ExecuteSqlQueryWithParams("spGetRoleDetailByRoleId", dictionaryobj);
            if (dt.Rows.Count > 0)
            {
                rolePermissionModel = new();
                var listData = SQLHelper.ConvertToGenericModelList<RolePermissionModel>(dt);
                rolePermissionModel.Id = listData[0].Id;
                rolePermissionModel.Name = listData[0].Name;
                rolePermissionModel.Description = listData[0].Description;
                rolePermissionModel.AllPermissions = listData[0].AllPermissions;
                if (rolePermissionModel.AllPermissions == false)
                {
                    rolePermissionModel.Roles = GetRolesListData(dt);
                }
                else
                {
                    rolePermissionModel.Roles = null;
                }
            }
            return rolePermissionModel;
        }
        public List<SubRolesModel> GetRolesListData(DataTable dt)
        {
            List<SubRolesModel> subRoleList = new List<SubRolesModel>();

            var listMain = SQLHelper.ConvertToGenericModelList<SubModel>(dt);
            var groupData = listMain.GroupBy(x => x.GroupId);
            foreach (var item in groupData)
            {
                var siteGroup = listMain.Where(x => x.GroupId == item.Key).GroupBy(x => x.SiteId);
                SubRolesModel subRole = new SubRolesModel();
                subRole.site = new List<RoleGrouping2SubSiteModel>();
                foreach (var itemSite in siteGroup)
                {
                    RoleGrouping2SubSiteModel roleGrouping2SubSiteModel = new RoleGrouping2SubSiteModel();
                    roleGrouping2SubSiteModel.SiteId = itemSite.Key;
                    roleGrouping2SubSiteModel.Languages = new List<RoleGrouping2CultureModel>();

                    var cultureGroup = listMain.Where(x => x.GroupId == item.Key && x.SiteId == itemSite.Key).GroupBy(x => x.CultureId);
                    foreach (var itemCulture in cultureGroup)
                    {
                        RoleGrouping2CultureModel roleGrouping2CultureModel = new RoleGrouping2CultureModel();
                        roleGrouping2CultureModel.CultureId = itemCulture.Key;
                        roleGrouping2CultureModel.Privileges = new List<RoleGrouping2PermissionModel>();

                        var privilegesGroup = listMain.Where(x => x.GroupId == item.Key && x.SiteId == itemSite.Key && x.CultureId == itemCulture.Key).GroupBy(x => x.PrivilegesId);
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
                }
                subRoleList.Add(subRole);
            }
            return subRoleList;
        }
    }
}
