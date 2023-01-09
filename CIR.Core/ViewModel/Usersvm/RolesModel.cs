using CIR.Core.Entities.Users;

namespace CIR.Core.ViewModel.Usersvm
{
    public class RolesModel
    {
        public List<Roles> RolesList { get; set; } = new();
        public int Count { get; set; }
    }
}
