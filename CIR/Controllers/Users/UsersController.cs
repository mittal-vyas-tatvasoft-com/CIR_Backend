using CIR.Common.CustomResponse;
using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIR.Controllers.Users
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user != null)
                {
                    return Ok(user);
                    //return Ok(new CustomResponse() { Result = true, Message = "Successfully got user data", ResponseData = JsonConvert.SerializeObject(user) });
                }
                return NotFound(new { message = "User not found!" });

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error : " + ex });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
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
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Error : " + ex + " Invalid input data" });
                }

            }

            return BadRequest();
        }

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

                return BadRequest(new { message = "Invalid input" });

            }

            return BadRequest(new { message = "Invalid input" });
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult UsersSP(int displayLength, int displayStart, int sortCol, string? search, string sortDir = "asc")
        {
            try
            {
                search ??= string.Empty;

                var userList = _userService.GetFilteredUsers(displayLength, displayStart, sortCol, sortDir, search);
                if(userList.Count > 0)
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> UsersLinq(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
        {
            try
            {
                search ??= string.Empty;

                var userList = await _userService.GetFilteredUsersLinq(displayLength, displayStart, sortCol, search, sortAscending);
                if(userList.Count > 0 )
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
