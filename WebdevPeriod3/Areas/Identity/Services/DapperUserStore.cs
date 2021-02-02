using Microsoft.AspNetCore.Identity;
using System;
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
    public class DapperUserStore : IUserPasswordStore<User>, IUserSecurityStampStore<User>
    {
        private readonly UserRepository _userRepository;

        public DapperUserStore(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
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

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
