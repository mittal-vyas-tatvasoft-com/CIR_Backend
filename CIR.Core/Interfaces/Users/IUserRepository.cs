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
        Task<User> CreateOrUpdateUser(User user);

        Task<List<User>> AllUsersList();

        Task<User> DeleteUser(int id);

    }
}
