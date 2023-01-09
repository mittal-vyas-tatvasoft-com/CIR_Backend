using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Core.Entities.User;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace CIR.Data.Data.Users
{
    public class UserRepository : ControllerBase, IUserRepository
    {
        #region PROPERTIES   
        private readonly CIRDbContext _CIRDBContext;
        #endregion

        #region CONSTRUCTOR
        public UserRepository(CIRDbContext context)
        {
            _CIRDBContext = context ??
               throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region METHODS
        /// <summary>
        /// fetches user based on input user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> user or null user if not found </returns>

        public async Task<User> GetUserById(int id)
        {
            var user = await _CIRDBContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// this meethod checks if user exists or not based on input user email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> if user exists true else false </returns>

        public async Task<Boolean> UserExists(string email)
        {
            var checkUserExist = await _CIRDBContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (checkUserExist != null && checkUserExist.Id > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used by create method and update method of user controller
        /// </summary>
        /// <param name="user"> new user data or update data for user </param>
        /// <returns> Ok status if its valid else unprocessable </returns>

        public async Task<IActionResult> CreateOrUpdateUser(User user)
        {
            User newUser = new()
            {
                Id = user.Id,
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
            }
            else
            {
                _CIRDBContext.Users.Add(newUser);
            }

            await _CIRDBContext.SaveChangesAsync();
            if (user.Email != null)
            {
                return Ok(new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated });
            }

            return UnprocessableEntity(new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity });
        }

        /// <summary>
        /// this metohd updates a column value and disables user
        /// </summary>
        /// <param name="id"></param>
        /// <returns> deleted/disabled user </returns>

        public async Task<IActionResult> DeleteUser(int id)
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
                return Ok(new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.NoContent, Result = true, Message = HttpStatusCodesMessages.NoContent, Data = user });
            }
            catch
            {
                return UnprocessableEntity(new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = user });
            }

        }

        /// <summary>
        /// This method retuns filtered user list using SP
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
        /// <returns> filtered list of users </returns>


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

            DataTable dt = SQLHelper.ExecuteSqlQueryWithParams("spGetFilteredUsers", dictionaryobj);

            UsersModel usersData = new();

            usersData.Count = Convert.ToInt32(dt.Rows[0]["TotalCount"].ToString());
            usersData.UsersList = SQLHelper.ConvertToGenericModelList<User>(dt);

            return usersData;
        }

        /// <summary>
        /// This method retuns filtered user list using LINQ
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
        /// <returns> filtered list of users </returns>


        public async Task<UsersModel> GetAllUsers(int displayLength, int displayStart, string sortCol, string? search, bool sortAscending = true)
        {
            UsersModel users = new();
            IQueryable<User> temp = users.UsersList.AsQueryable();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try
            {
                users.Count = _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).Count();

                temp = sortAscending ? _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderBy(x => EF.Property<object>(x, sortCol)).AsQueryable()
                                     : _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderByDescending(x => EF.Property<object>(x, sortCol)).AsQueryable();

                var sortedData = await temp.Skip(displayStart).Take(displayLength).ToListAsync();
                users.UsersList = sortedData;

                return users;
            }
            catch
            {
                return users;
            }
        }
        #endregion
    }
}
