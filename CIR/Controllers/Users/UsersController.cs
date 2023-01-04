using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace CIR.Controllers.Users
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// This method fetches single user data using user's Id
        /// </summary>
        /// <param name="id">user will be fetched according to this 'id'</param>
        /// <returns> user </returns>
        /// 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user != null)
                {
                    //return Ok(user); 
                    return Ok(new CustomResponse<User>() { StatusCode = HttpStatusCodes.Success, Result = true, Message = "Successfully got user data", Data = user });
                }
                return NotFound(new { message = "User not found!" });

            }
            catch (Exception ex)
            {               
                return BadRequest(new { message = "Error : " + ex });
            }
        }

        /// <summary>
        /// This method takes user details as parameters and creates user and returns that user
        /// </summary>
        /// <param name="user"> this object contains different parameters as details of a user </param>
        /// <returns > created user </returns>
        /// 

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
                        return Ok(new CustomResponse<string>() { Message = "user already exists" });
                    }
                    else
                    {
                        var newUser = await _userService.CreateOrUpdateUser(user);
                        if (newUser.Id > 0)
                        {
                            return Ok(newUser);
                        }
                        else
                        {
                            return BadRequest(new { message = "Error occured creating new user" });
                        }
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }

            }

            return BadRequest();
        }

        /// <summary>
        /// This method takes user details and updates the user
        /// </summary>
        /// <param name="user"> this object contains different parameters as details of a user </param>
        /// <returns> updated user </returns>
        /// 

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedUser = await _userService.CreateOrUpdateUser(user);
                    if (updatedUser.Id > 0)
                    {
                        return Ok(updatedUser);
                    }
                    else
                    {
                        return BadRequest(new { message = "Error occured updating user data" });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }

            }

            return BadRequest();
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
                var deletedUser = await _userService.DeleteUser(id);

                if (deletedUser.Id > 0)
                {
                    return Ok(deletedUser);
                }

                 _logger.LogError("invalid input user id as database was unable to find appropriate user");
                return BadRequest(new { message = "Invalid input" });

            }

            return BadRequest(new { message = "Invalid input" });
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

                var userList = _userService.GetFilteredUsers(displayLength, displayStart, sortCol, sortDir, search);
                if (userList.Count > 0)
                {
                    return Ok(userList);
                }

                return NotFound("Requested users were not found");

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error : " + ex });
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

                var userList = await _userService.GetFilteredUsersLinq(displayLength, displayStart, sortCol, search, sortAscending);
                if (userList.Count > 0)
                {
                    return Ok(userList);
                }

                return NotFound("Requested users were not found");

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error : " + ex });
            }
        }

    }
}
