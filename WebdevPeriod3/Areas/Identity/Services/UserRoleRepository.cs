using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    public class UserRoleRepository : BaseRepository
    {
        private readonly Expression<Func<UserRole, string>> ROLE_ID_SELECTOR = userRole => userRole.RoleId;
        private readonly Expression<Func<UserRole, string>> USER_ID_SELECTOR = userRole => userRole.UserId;
        private readonly Expression<Func<Role, string>> RIGHT_ROLE_ID_SELECTOR = role => role.Id;
        private readonly Expression<Func<User, string>> RIGHT_USER_ID_SELECTOR = user => user.Id;
        private readonly Expression<Func<Role, string>> RIGHT_ROLE_NAME_SELECTOR = role => role.Name;

        private readonly RoleRepository _roleRepository;

        public UserRoleRepository(IConfiguration configuration, RoleRepository roleRepository) : base(configuration)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<T>> GetRolesFieldByUserId<T>(string userId, Expression<Func<Role, T>> expression) =>
            WithConnection(connection => connection.QueryAsync<T>(
                // Select the expression, specifying a differing left-hand table
                $"{expression.ToSelectClause(typeof(UserRole).ToTableName())} " +
                // Join by role ID
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                // Filter by user ID
                $"AND {USER_ID_SELECTOR.ToKeyValuePair(nameof(userId))};",
                new { userId }));

        public Task<IEnumerable<User>> GetUsersByRoleName(string roleName) =>
            WithConnection(connection => connection.QueryAsync<User>(
                $"SELECT {typeof(User).ToTableName()}.* FROM {typeof(Role).ToTableName()} " +
                $"INNER {RIGHT_ROLE_ID_SELECTOR.ToJoinClause(ROLE_ID_SELECTOR)} " +
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                $"INNER {USER_ID_SELECTOR.ToJoinClause(RIGHT_USER_ID_SELECTOR)};",
                new { roleName }));

        public Task<bool> IsInRole(string userId, string roleName) =>
            WithConnection(connection => connection.ExecuteScalarAsync<bool>(
                $"SELECT EXISTS (" +
                $"SELECT 1 FROM {typeof(User).ToTableName()} " +
                $"INNER {RIGHT_USER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)} " +
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                $");",
                new { userId, roleName }));

        public Task RemoveUserFromRole(string userId, string roleName) =>
            WithConnection(connection => connection.ExecuteAsync(
                $"DELETE FROM {typeof(UserRole).ToTableName()} " +
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))};"));

        public async Task AddUserToRole(string userId, string roleName)
        {
            var roleId = await _roleRepository.GetFieldByNormalizedName(roleName.ToUpperInvariant(), role => role.Id);
            var userRole = new UserRole()
            {
                RoleId = roleId,
                UserId = userId
            };

            await WithConnection(connection => connection.ExecuteAsync(
                userRole.ToInsertQuery(),
                userRole));
        }
    }
}
