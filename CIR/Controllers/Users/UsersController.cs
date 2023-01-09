using CIR.Common.CustomResponse;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Users
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        #region PROPERTIES
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        #endregion

        #region CONSTRUCTOR
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns> 

        [HttpGet("{id}")]
        public async Task<CustomResponse<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user != null)
                {
                    return new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = user };
                }
                return new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = user };

            }
            catch
            {
                return new CustomResponse<User>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError };
            }
        }

        /// <summary>
        /// This method takes user details as parameters and creates user and returns that user
        /// </summary>
        /// <param name="user"> this object contains different parameters as details of a user </param>
        /// <returns > created user </returns>

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userExists = await _userService.UserExists(user.Email);
                    if (userExists)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "user already exists" });
                    }
                    else
                    {
                        return await _userService.CreateOrUpdateUser(user);
                    }

                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }

            }

            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        /// <summary>
        /// This method takes user details and updates the user
        /// </summary>
        /// <param name="user"> this object contains different parameters as details of a user </param>
        /// <returns> updated user </returns>

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return await _userService.CreateOrUpdateUser(user);
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }

            }

            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
        }

        /// <summary>
        /// This method disables user 
        /// </summary>
        /// <param name="id"> user will be disabled according to this id </param>
        /// <returns> disabled user </returns>

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                return await _userService.DeleteUser(id);
            }

            return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
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

        [HttpGet("[action]")]
        public IActionResult UsersSP(int displayLength, int displayStart, int sortCol, string? search, string sortDir = "asc")
        {
            try
            {
                search ??= string.Empty;

                var usersData = _userService.GetFilteredUsers(displayLength, displayStart, sortCol, sortDir, search);
                if (usersData.UsersList.Count > 0)
                {
                    return new JsonResult(new CustomResponse<UsersModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = usersData });
                }

                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested users were not found" });

            }
            catch (Exception ex)
            {
                _logger.LogError("database was unable to find appropriate users");
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
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

        [HttpGet]
        public async Task<IActionResult> UsersLinq(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
        {
            try
            {
                search ??= string.Empty;

                var usersData = await _userService.GetAllUsers(displayLength, displayStart, sortCol, search, sortAscending);
                if (usersData.UsersList.Count > 0)
                {
                    return new JsonResult(new CustomResponse<UsersModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = usersData });
                }

                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested users were not found" });

            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
