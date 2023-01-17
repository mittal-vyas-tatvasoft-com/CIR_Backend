using CIR.Common.CustomResponse;
using CIR.Common.Data;
using CIR.Common.Helper;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.Usersvm;
using Dapper;
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

        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var userDetail = await _CIRDBContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                return new JsonResult(new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = userDetail });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }

        }

        /// <summary>
        /// this meethod checks if user exists or not based on input user email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> if user exists true else false </returns>

        public async Task<Boolean> UserExists(string email, long id)
        {
            User checkUserExists;
            if (id == 0)
            {
                checkUserExists = await _CIRDBContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            }
            else
            {
                checkUserExists = await _CIRDBContext.Users.Where(x => x.Email == email && x.Id != id).FirstOrDefaultAsync();
            }
            if (checkUserExists != null)
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
            try
            {
                string result = "";
                using (DbConnection dbConnection = new DbConnection())
                {

                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@Id", user.Id);
                        parameters.Add("@UserName", user.UserName);
                        parameters.Add("@Password", user.Password);
                        parameters.Add("@Email", user.Email);
                        parameters.Add("@SalutationLookupItemId", user.SalutationLookupItemId);
                        parameters.Add("@FirstName", user.FirstName);
                        parameters.Add("@LastName", user.LastName);
                        parameters.Add("@RoleId", user.RoleId);
                        parameters.Add("@Enabled", true);
                        parameters.Add("@ResetRequired", false);
                        parameters.Add("@DefaultAdminUser", false);
                        parameters.Add("@TimeZone", user.TimeZone);
                        parameters.Add("@CultureLcid", user.CultureLcid);
                        parameters.Add("@EmployeeId", user.EmployeeId);
                        parameters.Add("@PhoneNumber", user.PhoneNumber);
                        parameters.Add("@ScheduledActiveChange", user.ScheduledActiveChange);
                        parameters.Add("@LoginAttempts", user.LoginAttempts);
                        parameters.Add("@CompanyName", user.CompanyName);
                        parameters.Add("@PortalThemeId", user.PortalThemeId);
                        result = Convert.ToString(connection.ExecuteScalar("spAddUpdateUsers", parameters, commandType: CommandType.StoredProcedure));
                    }
                }
                if (result != null)
                {
                    return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.CreatedOrUpdated, Result = true, Message = HttpStatusCodesMessages.CreatedOrUpdated, Data = result });
                }
                return UnprocessableEntity(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = false, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = "error" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// this metohd updates a column value and disables user
        /// </summary>
        /// <param name="id"></param>
        /// <returns> deleted/disabled user </returns>

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                User user = new()
                {
                    Id = id,
                    Enabled = false
                };

                _CIRDBContext.Entry(user).Property(x => x.Enabled).IsModified = true;
                await _CIRDBContext.SaveChangesAsync();
                return Ok(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Deleted, Data = "UserDetail deleted successfully." });
            }
            catch
            {
                return UnprocessableEntity(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "error" });
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


        public async Task<IActionResult> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
        {
            UsersViewModel users = new();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try
            {
                users.Count = _CIRDBContext.Users.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).Count();

                var sortData = await (from userData in _CIRDBContext.Users
                                      join roleData in _CIRDBContext.Roles on userData.RoleId equals roleData.Id
                                      join cultureData in _CIRDBContext.Cultures on userData.CultureLcid equals cultureData.Id
                                      select new UserModel()
                                      {
                                          Id = userData.Id,
                                          UserName = userData.UserName,
                                          Password = userData.Password,
                                          Email = userData.Email,
                                          SalutationLookupItemId = userData.SalutationLookupItemId,
                                          FirstName = userData.FirstName,
                                          LastName = userData.LastName,
                                          FullName = userData.FirstName + ' ' + userData.LastName,
                                          RoleId = userData.RoleId,
                                          RoleName = roleData.Name,
                                          Enabled = userData.Enabled,
                                          LastLogOn = userData.LastLogOn,
                                          CreatedOn = userData.CreatedOn,
                                          LastEditedOn = userData.LastEditedOn,
                                          ResetRequired = userData.ResetRequired,
                                          DefaultAdminUser = userData.DefaultAdminUser,
                                          TimeZone = userData.TimeZone,
                                          CultureLcid = userData.CultureLcid,
                                          CultureDisplayName = cultureData.DisplayName,
                                          CultureNativeName = cultureData.NativeName,
                                          EmployeeId = userData.EmployeeId,
                                          PhoneNumber = userData.PhoneNumber,
                                          ScheduledActiveChange = userData.ScheduledActiveChange,
                                          LoginAttempts = userData.LoginAttempts,
                                          CompanyName = userData.CompanyName,
                                          PortalThemeId = userData.PortalThemeId
                                      }).ToListAsync();
                if (sortAscending)
                {
                    sortData = sortData.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
                }
                else
                {
                    sortData = sortData.Where(y => y.UserName.Contains(search) || y.Email.Contains(search) || y.FirstName.Contains(search)).OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
                }
                users.UsersList = sortData;
                return new JsonResult(new CustomResponse<UsersViewModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = users });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }
        #endregion
    }
}
