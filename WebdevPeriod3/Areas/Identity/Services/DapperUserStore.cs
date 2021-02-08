using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using static WebdevPeriod3.Areas.Identity.Services.UserRepository;

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
        private readonly IdentityErrorDescriber _identityErrorDescriber;

        public DapperUserStore(UserRepository userRepository, UserRoleRepository userRoleRepository, IdentityErrorDescriber identityErrorDescriber)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _identityErrorDescriber = identityErrorDescriber;
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken) =>
            await _userRoleRepository.AddUserToRole(await GetField(user, user => user.Id), roleName);

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.Add(user);

                return IdentityResult.Success;
            } catch (DuplicateUserNameException)
            {
                return IdentityResult.Failed(_identityErrorDescriber.DuplicateUserName(user.UserName));
            }
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
            user.NormalizedUserName ?? await GetField(user, user => user.NormalizedUserName);

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken) =>
            user.PasswordHash ?? await GetField(user, user => user.PasswordHash);

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            if (user.Id != null)
                return (await _userRoleRepository.GetRolesFieldByUserId(user.Id, role => role.Name)).ToList();
            else if (user.NormalizedUserName != null)
                return (await _userRoleRepository.GetRolesFieldByNormalizedUserName(user.NormalizedUserName, role => role.Name)).ToList();
            else
                return (await _userRoleRepository.GetRolesFieldByNormalizedUserName(
                    user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    role => role.Name)).ToList();
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) =>
            user.SecurityStamp ?? await GetField(user, user => user.SecurityStamp);

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) =>
            user.Id ?? await GetField(user, user => user.Id);

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) =>
            user.UserName ?? await GetField(user, user => user.UserName);

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            var usersInRole = await _userRoleRepository.GetUsersByRoleName(roleName);

            return usersInRole.ToList();
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) =>
            await GetPasswordHashAsync(user, cancellationToken) != null;

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Id != null)
                return _userRoleRepository.IsInRoleByUserId(user.Id, roleName);
            if (user.NormalizedUserName != null)
                return _userRoleRepository.IsInRoleByNormalizedUserName(user.NormalizedUserName, roleName);
            else
                return _userRoleRepository.IsInRoleByNormalizedUserName(
                    user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    roleName);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Id != null)
                return _userRoleRepository.RemoveUserFromRoleByUserId(user.Id, roleName);
            else if (user.NormalizedUserName != null)
                return _userRoleRepository.RemoveUserFromRoleByNormalizedUserName(user.NormalizedUserName, roleName);
            else
                return _userRoleRepository.RemoveUserFromRoleByNormalizedUserName(
                    user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    roleName);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            await SetField(user, user => user.NormalizedUserName, normalizedName);

            user.NormalizedUserName = normalizedName;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            await SetField(user, user => user.PasswordHash, passwordHash);

            user.PasswordHash = passwordHash;
        }

        public async Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            await SetField(user, user => user.SecurityStamp, stamp);

            user.SecurityStamp = stamp;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            await SetField(user, user => user.UserName, userName);

            user.UserName = userName;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                await _userRepository.Update(user);

                // TODO: Decide what result we should return
                return IdentityResult.Success;
            } catch (DuplicateUserNameException)
            {
                return IdentityResult.Failed(_identityErrorDescriber.DuplicateUserName(user.UserName));
            }
        }

        private Task<T> GetField<T>(User user, Expression<Func<User, T>> expression)
        {
            if (user.Id != null)
                return _userRepository.GetFieldById(user.Id, expression);
            else
                return _userRepository.GetFieldByNormalizedUserName(
                    user.NormalizedUserName
                    ?? user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    expression);
        }

        private Task SetField<T>(User user, Expression<Func<User, T>> expression, T value)
        {
            if (user.Id != null)
                return _userRepository.UpdateFieldById(user.Id, expression, value);
            else
                return _userRepository.UpdateFieldByNormalizedUserName(
                    user.NormalizedUserName
                    ?? user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    expression,
                    value);
        }
    }
}
