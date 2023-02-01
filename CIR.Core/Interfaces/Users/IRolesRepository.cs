using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Core.Interfaces.Users
{
    public interface IRolesRepository
    {
        Task<IActionResult> GetAllRoles();
        Task<IActionResult> GetRoles(int displayLength, int displayStart, string? sortCol, string? search, bool sortAscending = true);
        Task<Boolean> RoleExists(string roleName, long id);
        Task<IActionResult> GetRoleDetailById(long roleId);
        Task<IActionResult> AddRole(RolePermissionModel roles);
        Task<IActionResult> DeleteRoles(long roleId);
        Task<IActionResult> RemoveSection(long groupId);

        Task<IActionResult> GetLanguagesListByRole();
        Task<IActionResult> GetRolePrivilegesList();
        Task<IActionResult> GetSubSiteList();
    }
}
