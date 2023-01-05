using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return  user;
        }

        public async Task<Boolean> UserExists(string email)
        {            
            return await _userRepository.UserExists(email);
        }


        public Task<IActionResult> CreateOrUpdateUser(User user)
        {
            return _userRepository.CreateOrUpdateUser(user);
        }
               
        public async Task<IActionResult> DeleteUser(int id)
        {           
            return await _userRepository.DeleteUser(id);
        }

        public UsersModel GetFilteredUsers(int displayLength, int displayStart, int sortCol, string sortDir, string search)
        {
            return _userRepository.GetFilteredUsers(displayLength, displayStart, sortCol, sortDir, search);    
        }

        public async Task<UsersModel> GetFilteredUsersLinq(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
        {
            return await _userRepository.GetFilteredUsersLinq(displayLength, displayStart, sortCol, search, sortAscending);
        }

    }
}
