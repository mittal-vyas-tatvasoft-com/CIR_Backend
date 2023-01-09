using CIR.Core.Entities.User;
using CIR.Core.ViewModel.User;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
    public interface IUserRepository
	{
		Task<User> GetUserById(int id);

		Task<Boolean> UserExists(string email);

		Task<IActionResult> CreateOrUpdateUser(User user);

		Task<IActionResult> DeleteUser(int id);

		UsersModel GetFilteredUsers(int displayLength, int displayStart, int sortCol, string sortDir, string search);

		Task<UsersModel> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true);

	}
}
