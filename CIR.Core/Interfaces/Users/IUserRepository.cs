using CIR.Core.Entities;
using CIR.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Core.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);

        Task<User> CreateOrUpdateUser(User user);

        Task<User> DeleteUser(int id);

        UsersModel GetFilteredUsers(int displayLength, int displayStart, int sortCol, string sortDir, string search);

        Task<UsersModel> GetFilteredUsersLinq(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true);

    }
}
