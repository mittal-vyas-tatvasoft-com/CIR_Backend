using CIR.Core.Entities.Users;

namespace CIR.Core.ViewModel
{
    public class RolePermissionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Boolean AllPermissions { get; set; }
        public List<SubRolesModel> Roles { get; set; }
    }
}