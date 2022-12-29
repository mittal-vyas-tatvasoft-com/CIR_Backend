using CIR.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Users
{
    public interface IUserService
    {
        Task<User> CreateOrUpdateUser(User user);

        Task<List<User>> GetAllUsers();

        Task<User> DeleteUser(int id);
    }
}
