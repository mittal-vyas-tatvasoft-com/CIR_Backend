using CIR.Common.CustomResponse;
using CIR.Common.Enums;
using CIR.Common.Helper;
using CIR.Core.Entities.Users;
using CIR.Core.Interfaces.Users;
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
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                return await _userService.GetUserById(id);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes user details as parameters and creates user and returns that user
        /// </summary>
        /// <param name="user"> this object contains different parameters as details of a user </param>
        /// <returns > created user </returns>

        [HttpPost("[action]")]
        [CustomPermissionFilter(RolePriviledgesEnums.User_Create)]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userExists = await _userService.UserExists(user.Email, user.Id);
                    if (userExists)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "User already exist!" });
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
        [CustomPermissionFilter(RolePriviledgesEnums.User_Update)]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userExists = await _userService.UserExists(user.Email, user.Id);
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
        /// This method disables user 
        /// </summary>
        /// <param name="id"> user will be disabled according to this id </param>
        /// <returns> disabled user </returns>

        [HttpDelete("[action]")]
        [CustomPermissionFilter(RolePriviledgesEnums.User_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id > 0)
                    {
                        return await _userService.DeleteUser(id);
                    }
                    return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "error" });
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
        [HttpGet("[action]")]
        [CustomPermissionFilter(RolePriviledgesEnums.User_List)]
        public async Task<IActionResult> GetAllUsersDetailBySP(int displayLength, int displayStart, string? sortCol, string? search, string? sortDir, int roleId, bool? enabled = null)
        {
            try
            {
                search ??= string.Empty;
                return await _userService.GetAllUsersDetailBySP(displayLength, displayStart, sortCol, search, sortDir, roleId, enabled);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
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

        [HttpGet]
        [CustomPermissionFilter(RolePriviledgesEnums.User_List)]
        public async Task<IActionResult> UsersLinq(int displayLength, int displayStart, string? sortCol, string? search, int roleId, bool? enabled = null, bool sortAscending = true)
        {
            try
            {
                search ??= string.Empty;
                return await _userService.GetAllUsers(displayLength, displayStart, sortCol, search, roleId, enabled, sortAscending);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        #endregion
    }
}
