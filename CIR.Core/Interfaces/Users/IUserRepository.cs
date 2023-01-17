using CIR.Core.Entities.Users;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
	public interface IUserRepository
	{
		Task<IActionResult> GetUserById(int id);

		Task<Boolean> UserExists(string email, long id);

		Task<IActionResult> CreateOrUpdateUser(User user);

		Task<IActionResult> DeleteUser(int id);

		UsersModel GetFilteredUsers(int displayLength, int displayStart, int sortCol, string sortDir, string search);

		Task<IActionResult> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true);

	}
}
