using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIR.Data.Data.Users
{
    public class UserRepository: IUserRepository
    {
        private readonly CIRDbContext _CIRDBContext;

        public UserRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> CreateOrUpdateUser(User user)
        {       
            User newUser = new()
            {   
                Id= user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = 1,
                Enabled = true,
                CreatedOn = DateTime.Now,
                ResetRequired = false,
                DefaultAdminUser = false,
                CultureLcid = 1,
                TimeZone = user.TimeZone,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber
            };

            if (user.Id > 0)
            {
                _CIRDBContext.Users.Update(newUser);
                await _CIRDBContext.SaveChangesAsync();
                return newUser;
            }
            else
            {
                _CIRDBContext.Users.Add(newUser);
                await _CIRDBContext.SaveChangesAsync();
                return newUser;
            }

            
        }

        public async Task<List<User>> AllUsersList()
        {
            var list = await _CIRDBContext.Users.ToListAsync();            
            return  list;
        }

        public async Task<User> DeleteUser(int id)
        {
            User user = new()
            {
                Id = id,
                Enabled = false
            };

            try
            {
                _CIRDBContext.Entry(user).Property(x => x.Enabled).IsModified = true;
                await _CIRDBContext.SaveChangesAsync();
                return user;
            }
            catch
            {
                return new User();
            }
            
        }
    }
}
