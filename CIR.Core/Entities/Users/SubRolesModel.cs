namespace CIR.Core.Entities.Users
{
    public class SubRolesModel
    {
        public long groupId { get; set; }
        public List<RoleGrouping2SubSiteModel> site { get; set; }
    }
    public partial class RoleGrouping2SubSiteModel
    {
        public long SiteId { get; set; }
        public List<RoleGrouping2CultureModel> Languages { get; set; }
    }
    public partial class RoleGrouping2CultureModel
    {
        public long CultureId { get; set; }
        public List<RoleGrouping2PermissionModel> Privileges { get; set; }
    }
    public partial class RoleGrouping2PermissionModel
    {
        public long PrivilegesId { get; set; }
    }
    public partial class SubModel
    {
        public long GroupId { get; set; }
        public long SiteId { get; set; }
        public long CultureId { get; set; }
        public long PrivilegesId { get; set; }
    }
}
