using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;

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
        private readonly DapperTransactionService _dapperTransactionService;

        public DapperRoleStore(RoleRepository roleRepository, DapperTransactionService dapperTransactionService)
        {
            _roleRepository = roleRepository;
            _dapperTransactionService = dapperTransactionService;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            _roleRepository.Add(role);

            await _dapperTransactionService.RunOperations(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            _roleRepository.Delete(role);

            await _dapperTransactionService.RunOperations(cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken) =>
            _roleRepository.FindById(roleId);

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) =>
            _roleRepository.FindByNormalizedName(normalizedRoleName);

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            _roleRepository.Update(role);

            await _dapperTransactionService.RunOperations(cancellationToken);

            return IdentityResult.Success;
        }
    }
}
