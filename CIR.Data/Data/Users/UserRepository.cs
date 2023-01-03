using Azure;
using CIR.Common.Data;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<User> GetUserById(int id)
        {
            var user = await _CIRDBContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            return user;
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

        public UsersModel GetFilteredUsers(int displayLength, int displayStart, int sortCol, string sortDir, string? search)
        {
            var dictionaryobj = new Dictionary<string, object>
            {
                { "DisplayLength", displayLength },
                { "DisplayStart", displayStart },
                { "SortCol", sortCol },
                { "SortDir", sortDir },
                { "Search", search }
            };

            DataTable dt = Helper.SQLHelper.ExecuteSqlQueryWithParams("spGetFilteredUsers", dictionaryobj);

            UsersModel usersData = new();

            usersData.Count = Convert.ToInt32(dt.Rows[0]["TotalCount"].ToString());
            usersData.UsersList = Helper.SQLHelper.ConvertToGenericModelList<User>(dt);
                       
            return usersData;
        }

        public async Task<UsersModel> GetFilteredUsersLinq(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
        {
            UsersModel users = new();
            IQueryable<User> temp =  users.UsersList.AsQueryable();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try
            {
                users.Count = _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).Count();

                temp = sortAscending ? _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderBy(x => EF.Property<object>(x, sortCol)).AsQueryable()
                                     : _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderByDescending(x => EF.Property<object>(x, sortCol)).AsQueryable();

                var sortedData = await temp.Skip(displayStart * displayLength).Take(displayLength).ToListAsync();
                users.UsersList = sortedData;

                return users;
            }
            catch
            {
                return users;
            }
            
        }



    }
}
