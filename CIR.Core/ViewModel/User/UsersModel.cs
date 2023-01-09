using CIR.Core.Entities.User;

namespace CIR.Core.ViewModel.User
{
    public class UsersModel
    {
        public List<User> UsersList { get; set; } = new();

        public int Count { get; set; }
    }
}
