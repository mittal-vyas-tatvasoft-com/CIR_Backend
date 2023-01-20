using CIR.Core.ViewModel;
using CIR.Core.ViewModel.Usersvm;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
    public interface IRolesService
    {
        Task<IActionResult> GetRoles();
        Task<RolesModel> GetAllRoles(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true);
        Task<Boolean> RoleExists(string rolename, long id);
        Task<IActionResult> GetRoleDetailById(int roleId);
        Task<IActionResult> AddRole(RolePermissionModel roles);
        Task<IActionResult> DeleteRoles(long roleId);

        Task<IActionResult> RemoveSection(long groupId);
    }
}
