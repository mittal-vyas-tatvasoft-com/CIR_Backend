using CIR.Core.Entities;
using CIR.Core.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CIR.Controllers.Users
{
    [Route("api/User")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var userList = await _userService.GetAllUsers();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error : " + ex });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
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
                catch(Exception ex)
                {
                    return BadRequest(new { message = "Error : " +ex +" Invalid input data" });
                }
                
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id > 0)
            {
                var deletedUser = await _userService.DeleteUser(id);

                if(deletedUser.Id > 0) 
                {
                    return Ok(deletedUser);
                }

                return BadRequest(new { message = "Invalid input" });

            }
            
            return BadRequest(new { message = "Invalid input" });
        }
    }
}
