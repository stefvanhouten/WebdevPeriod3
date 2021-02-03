using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    // TODO: Decide whether or not to implement IQueryableUserStore<User>.
    /// <summary>
    /// A Dapper-based user store implementation
    /// </summary>
    public class DapperUserStore : IUserPasswordStore<User>, IUserSecurityStampStore<User>, IUserRoleStore<User>
    {
        private readonly UserRepository _userRepository;
        private readonly UserRoleRepository _userRoleRepository;

        public DapperUserStore(UserRepository userRepository, UserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken) =>
            _userRoleRepository.AddUserToRole(user.Id, roleName);

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Add(user);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Delete(user);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        // TODO: Add support for cancellation tokens
        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            _userRepository.FindById(userId);

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
            _userRepository.FindByNormalizedUserName(normalizedUserName);

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken) =>
            user.NormalizedUserName ?? await _userRepository.GetFieldById(user.Id, user => user.NormalizedUserName);

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken) =>
            user.PasswordHash ?? await _userRepository.GetFieldById(user.Id, user => user.PasswordHash);

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            var roleNames = await _userRoleRepository.GetRolesFieldByUserId(user.Id, role => role.Name);

            return roleNames.ToList();
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) =>
            user.SecurityStamp ?? await _userRepository.GetFieldById(user.Id, user => user.SecurityStamp);

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) =>
            user.Id ?? await _userRepository.GetFieldByNormalizedUserName(user.NormalizedUserName, user => user.Id);

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) =>
            user.UserName ?? await _userRepository.GetFieldById(user.Id, user => user.UserName);

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var usersInRole = await _userRoleRepository.GetUsersByRoleName(roleName);

            return usersInRole.ToList();
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) =>
            user.PasswordHash != null || await _userRepository.GetFieldById(user.Id, user => user.PasswordHash) == null;

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken) =>
            _userRoleRepository.IsInRole(user.Id, roleName);

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken) =>
            _userRoleRepository.RemoveUserFromRole(user.Id, roleName);

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateFieldById(user.Id, user => user.NormalizedUserName, normalizedName);

            user.NormalizedUserName = normalizedName;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateFieldById(user.Id, user => user.PasswordHash, passwordHash);

            user.PasswordHash = passwordHash;
        }

        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateFieldById(user.Id, user => user.SecurityStamp, stamp);

            user.SecurityStamp = stamp;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateFieldById(user.Id, user => user.UserName, userName);

            user.UserName = userName;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Update(user);

            // TODO: Decide what result we should return
            return IdentityResult.Success;
        }
    }
}
