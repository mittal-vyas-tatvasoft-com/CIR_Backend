using CIR.Core.Entities.User;

namespace CIR.Core.ViewModel.User
{
    public class RolesModel
    {
        public List<Roles> RolesList { get; set; } = new();
        public int Count { get; set; }
    }
}
