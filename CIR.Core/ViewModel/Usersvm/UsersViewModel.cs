namespace CIR.Core.ViewModel.Usersvm
{
    public class UsersViewModel
    {
        public List<UserModel> UsersList { get; set; } = new();

        public int Count { get; set; }
    }
}
