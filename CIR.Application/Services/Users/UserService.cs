using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using System;
using System.Collections.Generic;
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

        public Task<User> CreateOrUpdateUser(User user)
        {
            return _userRepository.CreateOrUpdateUser(user);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var list = _userRepository.GetAllUsers();
            return  await list;
        }

        public async Task<User> DeleteUser(int id)
        {           
            return await _userRepository.DeleteUser(id);
        }
    }
}
