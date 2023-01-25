using CIR.Core.Entities.Users;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
    public interface IUserService
    {
        Task<IActionResult> GetUserById(int id);

        Task<Boolean> UserExists(string email, long id);

        Task<IActionResult> CreateOrUpdateUser(User user);

        Task<IActionResult> DeleteUser(int id);

        Task<IActionResult> GetAllUsersDetailBySP(int displayLength, int displayStart, string? sortCol, string? search, string? sortDir, int roleId, bool? enabled = null);

        Task<IActionResult> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, int roleId, bool? enabled = null, bool sortAscending = true);
    }
}
