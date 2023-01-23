using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<Boolean> UserExists(string email, long id)
        {
            return await _userRepository.UserExists(email, id);
        }


        public Task<IActionResult> CreateOrUpdateUser(User user)
        {
            return _userRepository.CreateOrUpdateUser(user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }

        public Task<IActionResult> GetAllUsersDetailBySP(int displayLength, int displayStart, string sortCol, string sortDir, string search, int roleId, bool? enabled = null)
        {
            return _userRepository.GetAllUsersDetailBySP(displayLength, displayStart, sortCol, sortDir, search, roleId, enabled);
        }

        public async Task<IActionResult> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, int roleId, bool? enabled = null, bool sortAscending = true)
        {
            return await _userRepository.GetAllUsers(displayLength, displayStart, sortCol, search, roleId, enabled, sortAscending);
        }
    }
}
