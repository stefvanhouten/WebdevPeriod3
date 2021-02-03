﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    // TODO: Decide whether or not to implement IQueryableUserStore<User>.
    /// <summary>
    /// A Dapper-based user store implementation
    /// </summary>
    public class DapperUserStore : IUserPasswordStore<User>, IUserSecurityStampStore<User>
    {
        private readonly UserRepository _userRepository;

        public DapperUserStore(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken) =>
            user.SecurityStamp ?? await _userRepository.GetFieldById(user.Id, user => user.SecurityStamp);

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) =>
            user.Id ?? await _userRepository.GetFieldByNormalizedUserName(user.NormalizedUserName, user => user.Id);

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken) =>
            user.UserName ?? await _userRepository.GetFieldById(user.Id, user => user.UserName);

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken) =>
            user.PasswordHash != null || await _userRepository.GetFieldById(user.Id, user => user.PasswordHash) == null;

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
