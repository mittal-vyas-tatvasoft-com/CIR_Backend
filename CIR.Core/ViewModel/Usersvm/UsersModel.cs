using CIR.Core.Entities.Users;

namespace CIR.Core.ViewModel.Usersvm
{
	public class UsersModel
	{
		public List<User> UsersList { get; set; } = new();

		public int Count { get; set; }
	}
}
