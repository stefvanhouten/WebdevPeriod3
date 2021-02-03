using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    // TODO: Decide whether to implement IQueryableRoleStore<Role>
    // TODO: Decide whether to implement IRoleClaimStore<Role>
    /// <summary>
    /// A Dapper-based role store implementation
    /// </summary>
    public class DapperRoleStore : IRoleStore<Role>
    {
        private readonly RoleRepository _roleRepository;

        public DapperRoleStore(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Add(role);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Delete(role);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
            _roleRepository.FindById(roleId);

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
            _roleRepository.FindByNormalizedName(normalizedRoleName);

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            role.NormalizedName ?? await _roleRepository.GetFieldById(role.Id, role => role.NormalizedName);

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) =>
            role.Id ?? await _roleRepository.GetFieldByNormalizedName(role.NormalizedName, role => role.NormalizedName);

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken) =>
            role.Name ?? await _roleRepository.GetFieldById(role.Id, role => role.Name);

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            await _roleRepository.UpdateFieldById(role.Id, normalizedName, role => role.NormalizedName);

            role.NormalizedName = normalizedName;
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            await _roleRepository.UpdateFieldById(role.Id, roleName, role => role.Name);

            role.Name = roleName;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            await _roleRepository.Update(role);

            return IdentityResult.Success;
        }
    }
}
