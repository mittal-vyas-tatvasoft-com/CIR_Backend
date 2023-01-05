using CIR.Core.Entities;

namespace CIR.Core.ViewModel
{
    public class UsersModel
    {
        public List<User> UsersList { get; set; } = new();

        public int Count { get; set; }
    }
}
