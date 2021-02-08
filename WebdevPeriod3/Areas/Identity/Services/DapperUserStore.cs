using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;
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
        private readonly DapperTransactionService _dapperTransactionService;

        public DapperUserStore(UserRepository userRepository, UserRoleRepository userRoleRepository, DapperTransactionService dappertransactionservice, IdentityErrorDescriber identityErrorDescriber)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _identityErrorDescriber = identityErrorDescriber;
            _dapperTransactionService = dappertransactionservice;
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException(nameof(roleName));
            }

            await _userRoleRepository.AddUserToRole(user.Id, roleName);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _userRepository.Add(user);

                await _dapperTransactionService.RunOperations(cancellationToken);

                return IdentityResult.Success;
            }
            catch (DuplicateUserNameException)
            {
                return IdentityResult.Failed(_identityErrorDescriber.DuplicateUserName(user.UserName));
            }
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            _userRepository.Delete(user);

            await _dapperTransactionService.RunOperations(cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose() { }

        // TODO: Add support for cancellation tokens
        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            _userRepository.FindById(userId);

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) =>
            _userRepository.FindByNormalizedUserName(normalizedUserName);

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

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

        public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

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
                _userRoleRepository.RemoveUserFromRoleByUserId(user.Id, roleName);
            else if (user.NormalizedUserName != null)
                _userRoleRepository.RemoveUserFromRoleByNormalizedUserName(user.NormalizedUserName, roleName);
            else
                _userRoleRepository.RemoveUserFromRoleByNormalizedUserName(
                    user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException(
                        "The provided user has to have a non-null ID, normalized user name, or user name."),
                    roleName);

            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.SecurityStamp = stamp ?? throw new ArgumentNullException(nameof(stamp));
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _userRepository.Update(user);

                await _dapperTransactionService.RunOperations(cancellationToken);

                // TODO: Decide what result we should return
                return IdentityResult.Success;
            }
            catch (DuplicateUserNameException)
            {
                return IdentityResult.Failed(_identityErrorDescriber.DuplicateUserName(user.UserName));
            }
        }
    }
}
