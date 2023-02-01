using CIR.Core.Interfaces.Users;
using CIR.Core.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Application.Services.Users
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<IActionResult> GetAllRoles()
        {
            return await _rolesRepository.GetAllRoles();
        }

        public async Task<IActionResult> GetRoles(int displayLength, int displayStart, string? sortCol, string search, bool sortAscending = true)
        {
            return await _rolesRepository.GetRoles(displayLength, displayStart, sortCol, search, sortAscending);
        }

        public async Task<Boolean> RoleExists(string roleName, long id)
        {
            return await _rolesRepository.RoleExists(roleName, id);
        }

        public async Task<IActionResult> GetRoleDetailById(long roleId)
        {
            return await _rolesRepository.GetRoleDetailById(roleId);
        }
        public async Task<IActionResult> AddRole(RolePermissionModel roles)
        {
            return await _rolesRepository.AddRole(roles);
        }

        public async Task<IActionResult> DeleteRoles(long roleId)
        {
            return await _rolesRepository.DeleteRoles(roleId);
        }

        public async Task<IActionResult> RemoveSection(long groupId)
        {
            return await _rolesRepository.RemoveSection(groupId);
        }

        public async Task<IActionResult> GetLanguagesListByRole()
        {
            return await _rolesRepository.GetLanguagesListByRole();
        }

        public async Task<IActionResult> GetRolePrivilegesList()
        {
            return await _rolesRepository.GetRolePrivilegesList();
        }
        public async Task<IActionResult> GetSubSiteList()
        {
            return await _rolesRepository.GetSubSiteList();
        }
    }
}
