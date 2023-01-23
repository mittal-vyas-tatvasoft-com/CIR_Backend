﻿namespace CIR.Data.Data.Users
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
        /// This method retuns filtered user list using Store Procedure
        /// </summary>
        /// <param name="displayLength">how many row/data we want to fetch(for pagination)</param>
        /// <param name="displayStart">from which row we want to fetch(for pagination)</param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search">word that we want to search in user table</param>
        /// <param name="sortDir">'asc' or 'desc' direction for sort </param>
        /// <param name="roleId">sorting roleid wise</param>
        /// <param name="enabled">sorting enable wise</param>
        /// <returns></returns>
        public async Task<IActionResult> GetAllUsersDetailBySP(int displayLength, int displayStart, string? sortCol, string? search, string? sortDir, int roleId, bool? enabled = null)
        {
            UsersViewModel users = new();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }
            try
            {
                List<UserModel> sortData;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("DisplayLength", displayLength);
                        parameters.Add("DisplayStart", displayStart);
                        parameters.Add("SortCol", sortCol);
                        parameters.Add("Search", search);
                        parameters.Add("SortDir", sortDir);
                        parameters.Add("RoleId", roleId);
                        parameters.Add("Enabled", enabled);
                        sortData = connection.Query<UserModel>("spGetFilteredUsersList", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                sortData = sortData.ToList();
                users.Count = sortData[0].TotalCount;
                users.UsersList = sortData;
                return new JsonResult(new CustomResponse<UsersViewModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = users });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }

        /// <summary>
        /// This method retuns filtered user list using Linq
        /// </summary>
        /// <param name="displayLength">how many row/data we want to fetch(for pagination)</param>
        /// <param name="displayStart">from which row we want to fetch(for pagination)</param>
        /// <param name="sortCol">name of column which we want to sort</param>
        /// <param name="search">word that we want to search in user table</param>
        /// <param name="roleId">sorting role id wise</param>
        /// <param name="enabled">sorting enable wise</param>
        /// <param name="sortAscending">'asc' or 'desc' direction for sort</param>
        /// <returns></returns>

        public async Task<IActionResult> GetAllUsers(int displayLength, int displayStart, string? sortCol, string search, int roleId, bool? enabled = null, bool sortAscending = true)
        {
            UsersViewModel users = new();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }

            try
            {
                List<UserModel> sortData;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("Search", search);
                        sortData = connection.Query<UserModel>("spGetUserDetailLists", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                if (roleId != 0)
                {
                    sortData = sortData.Where(y => y.RoleId == roleId).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
                }
                if (enabled != null)
                {
                    sortData = sortData.Where(y => y.Enabled == enabled).OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).ToList();
                }
                sortData = sortData.ToList();
                users.Count = sortData.Count();
                if (sortAscending)
                {
                    sortData = sortData.OrderBy(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
                }
                else
                {
                    sortData = sortData.OrderByDescending(x => x.GetType().GetProperty(sortCol).GetValue(x, null)).Skip(displayStart).Take(displayLength).ToList();
                }
                users.UsersList = sortData;
                return new JsonResult(new CustomResponse<UsersViewModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = users });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.UnprocessableEntity, Result = true, Message = HttpStatusCodesMessages.UnprocessableEntity, Data = ex });
            }
        }

        public async Task<IActionResult> GetAllUsersDetailBySP(int displayLength, int displayStart, string? sortCol, string? search, string? sortDir, int roleId, bool? enabled = null)
        {
            UsersViewModel users = new();

            if (string.IsNullOrEmpty(sortCol))
            {
                sortCol = "Id";
            }
            try
            {
                List<UserModel> sortData;
                using (DbConnection dbConnection = new DbConnection())
                {
                    using (var connection = dbConnection.Connection)
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("DisplayLength", displayLength);
                        parameters.Add("DisplayStart", displayStart);
                        parameters.Add("SortCol", sortCol);
                        parameters.Add("Search", search);
                        parameters.Add("SortDir", sortDir);
                        parameters.Add("RoleId", roleId);
                        parameters.Add("Enabled", enabled);
                        sortData = connection.Query<UserModel>("spGetFilteredUsersList", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
                sortData = sortData.ToList();
                users.Count = sortData[0].TotalCount;
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
