using CIR.Common.CustomResponse;
using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        #region PROPETRTIES 
        private readonly IRolesService _rolesService;
        private readonly ILogger<RolesController> _logger;

        #endregion

        #region CONSTRUCTOR
        public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
        {
            _rolesService = rolesService;
            _logger = logger;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// This method takes a get Roles list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllroles")]
        public async Task<IActionResult> GetAllroles()
        {
            try
            {
                return await _rolesService.GetRoles();
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }
        /// <summary>
        /// This method retuns filtered roles list using SP
        /// </summary>
        /// <param name="displayLength"> how many row/data we want to fetch(for pagination) </param>
        /// <param name="displayStart"> from which row we want to fetch(for pagination) </param>
        /// <param name="sortCol"> name of column which we want to sort</param>
        /// <param name="search"> word that we want to search in user table </param>
        /// <param name="sortDir"> 'asc' or 'desc' direction for sort </param>
        /// <returns> filtered list of roles </returns>
        [HttpGet("GetFilterdRoles")]
        public async Task<IActionResult> GetAllRoleFiltered(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true)
        {
            try
            {
                search ??= string.Empty;

                var rolesModel = await _rolesService.GetAllRolesFilterd(displayLength, displayStart, sortCol, search, sortAscending);

                if (rolesModel.RolesList.Count > 0)
                {
                    return new JsonResult(new CustomResponse<RolesModel>() { StatusCode = (int)HttpStatusCodes.Success, Result = true, Message = HttpStatusCodesMessages.Success, Data = rolesModel });
                }
                return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Requested roles were not found" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> Get(int roleId)
        {
            try
            {
                return await _rolesService.GetRoleDetailById(roleId);
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
            }
        }

        /// <summary>
        /// This method takes roles details and add role
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(RolePermissionModel roles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = await _rolesService.RoleExists(roles.Name, roles.Id);
                    if (isExist)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Role already exists" });
                    }
                    else
                    {
                        return await _rolesService.AddRole(roles);
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
        }

        /// <summary>
        /// This method takes roles details and update role
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(RolePermissionModel roles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isExist = await _rolesService.RoleExists(roles.Name, roles.Id);
                    if (isExist)
                    {
                        return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Role already exists" });
                    }
                    else
                    {
                        return await _rolesService.AddRole(roles);
                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.InternalServerError, Data = ex });
                }
            }
            return new JsonResult(new CustomResponse<string>() { StatusCode = (int)HttpStatusCodes.BadRequest, Result = false, Message = HttpStatusCodesMessages.BadRequest, Data = "Error" });
        }

        /// <summary>
        /// This method takes roles details and delete role
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(long roleId)
        {
            try
            {
                if (roleId > 0)
                {
                    return await _rolesService.DeleteRoles(roleId);
                }
                return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = ex });
            }
        }

        /// <summary>
        /// This method takes remove role id wise section
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("RemoveSection/{groupId}")]
        public async Task<IActionResult> RemoveSection(long groupId)
        {
            try
            {
                if (groupId > 0)
                {
                    return await _rolesService.RemoveSection(groupId);
                }
                return new JsonResult(new CustomResponse<String>() { StatusCode = (int)HttpStatusCodes.NotFound, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = "Invalid input id" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new CustomResponse<Exception>() { StatusCode = (int)HttpStatusCodes.InternalServerError, Result = false, Message = HttpStatusCodesMessages.NotFound, Data = ex });
            }
        }
        #endregion
    }
}
