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

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        // HACK: Please remove this warning when we start implementing the methods below.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's normalized user name from the database.
            return user.NormalizedUserName ?? throw new NotImplementedException();
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's password hash from the database.
            return user.PasswordHash ?? throw new NotImplementedException();
        }

        public async Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's security stamp from the database.
            return user.SecurityStamp ?? throw new NotImplementedException();
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's ID from the database.
            return user.Id ?? throw new NotImplementedException();
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's username from the database.
            return user.UserName ?? throw new NotImplementedException();
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            // TODO: Fetch the user's password hash from the database and check whether it isn't null.
            return user.PasswordHash != null ? true : throw new NotImplementedException();
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

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
