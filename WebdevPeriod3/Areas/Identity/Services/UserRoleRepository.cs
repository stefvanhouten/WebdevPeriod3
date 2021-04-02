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
    public class UserRoleRepository : TransactionRepositoryBase
    {
        /// <summary>
        /// Selects the role ID from a user role
        /// </summary>
        private readonly Expression<Func<UserRole, string>> ROLE_ID_SELECTOR = userRole => userRole.RoleId;
        /// <summary>
        /// Selects the user ID from a user role
        /// </summary>
        private readonly Expression<Func<UserRole, string>> USER_ID_SELECTOR = userRole => userRole.UserId;

        /// <summary>
        /// Selects the role ID from a role
        /// </summary>
        private readonly Expression<Func<Role, string>> RIGHT_ROLE_ID_SELECTOR = role => role.Id;
        /// <summary>
        /// Selects the role name from a role
        /// </summary>
        private readonly Expression<Func<Role, string>> RIGHT_ROLE_NAME_SELECTOR = role => role.Name;

        /// <summary>
        /// Selects the user ID from a user
        /// </summary>
        private readonly Expression<Func<User, string>> RIGHT_USER_ID_SELECTOR = user => user.Id;
        /// <summary>
        /// Selects the user name from a user
        /// </summary>
        private readonly Expression<Func<User, string>> RIGHT_NORMALIZED_USER_NAME_SELECTOR = user => user.NormalizedUserName;

        private readonly RoleRepository _roleRepository;

        public UserRoleRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration, RoleRepository roleRepository) : base(dapperTransactionService, configuration)
        {
            _roleRepository = roleRepository;
        }

        public Task<IEnumerable<T>> GetRolesFieldByUserId<T>(string userId, Expression<Func<Role, T>> expression) =>
            WithConnection(connection => connection.QueryAsync<T>(
                // Select the expression, specifying the UserRoles table as the left-hand table
                $"{expression.ToSelectClause(typeof(UserRole).ToTableName())} " +
                // Join user roles to roles by role ID...
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                // ...whereever the user ID matches the provided user ID
                $"AND {USER_ID_SELECTOR.ToKeyValuePair(nameof(userId))};",
                new { userId }));

        public Task<IEnumerable<T>> GetRolesFieldByNormalizedUserName<T>(string normalizedName, Expression<Func<Role, T>> expression) =>
            WithConnection(connection => connection.QueryAsync<T>(
                // Select the expression, specifying the Users table as the left-hand table
                $"{expression.ToSelectClause(typeof(User).ToTableName())} " +
                $"INNER {RIGHT_USER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)} " +
                $"AND {RIGHT_NORMALIZED_USER_NAME_SELECTOR.ToKeyValuePair(nameof(normalizedName))} " +
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)};"));

        public Task<IEnumerable<User>> GetUsersByRoleName(string roleName) =>
            WithConnection(connection => connection.QueryAsync<User>(
                // Select all fields from the Users table, specifying the Roles table as the left-hand table
                $"SELECT {typeof(User).ToTableName()}.* FROM {typeof(Role).ToTableName()} " +
                // Join roles to user roles by role ID...
                $"INNER {RIGHT_ROLE_ID_SELECTOR.ToJoinClause(ROLE_ID_SELECTOR)} " +
                // ...whereever the role name matches the provided role name
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                // Join user roles to users by user ID
                $"INNER {USER_ID_SELECTOR.ToJoinClause(RIGHT_USER_ID_SELECTOR)};",
                new { roleName }));

        public Task<bool> IsInRoleByUserId(string userId, string roleName) =>
            WithConnection(connection => connection.ExecuteScalarAsync<bool>(
                $"SELECT EXISTS (" +
                $"SELECT 1 FROM {typeof(User).ToTableName()} " +
                // Join users to user roles by user ID
                $"INNER {RIGHT_USER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)} " +
                $"AND {RIGHT_USER_ID_SELECTOR.ToKeyValuePair(nameof(userId))} " +
                // Join user roles to roles by role ID...
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                // ...whereever the role name matches the provided role name
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                $");",
                new { userId, roleName }));

        public Task<bool> IsInRoleByNormalizedUserName(string normalizedUserName, string roleName) =>
            WithConnection(connection => connection.ExecuteScalarAsync<bool>(
                $"SELECT EXISTS (" +
                $"SELECT 1 FROM {typeof(User).ToTableName()} " +
                $"INNER {RIGHT_USER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)} " +
                $"AND {RIGHT_NORMALIZED_USER_NAME_SELECTOR.ToKeyValuePair(nameof(normalizedUserName))} " +
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(roleName)}" +
                $");",
                new { normalizedUserName, roleName }));

        public void RemoveUserFromRoleByUserId(string userId, string roleName) =>
            AddOperation((connection, transaction) => connection.ExecuteAsync(
                // Delete a user role
                $"DELETE {typeof(UserRole).ToTableName()} FROM {typeof(UserRole).ToTableName()} " +
                // Join roles to user roles by role ID...
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                // ...whereever the role name matches the provided role name...
                $"WHERE {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                // ...and the user ID matches the provided user ID
                $"AND {USER_ID_SELECTOR.ToKeyValuePair(nameof(userId))};",
                new { userId, roleName }, transaction));

        public void RemoveUserFromRoleByNormalizedUserName(string normalizedUserName, string roleName) =>
            AddOperation((connection, transaction) => connection.ExecuteAsync(
                // Delete a user role
                $"DELETE FROM {typeof(UserRole).ToTableName()} " +
                // Join roles to user roles by role ID...
                $"INNER {ROLE_ID_SELECTOR.ToJoinClause(RIGHT_ROLE_ID_SELECTOR)} " +
                // ...whereever the role name matches the provided role name
                $"AND {RIGHT_ROLE_NAME_SELECTOR.ToKeyValuePair(nameof(roleName))} " +
                // Join users to user roles by user ID...
                $"INNER {USER_ID_SELECTOR.ToJoinClause(RIGHT_USER_ID_SELECTOR)} " +
                // ...wherever the normalized user name matches the provided normalized user name
                $"AND {RIGHT_NORMALIZED_USER_NAME_SELECTOR.ToKeyValuePair(nameof(normalizedUserName))};",
                new { normalizedUserName, roleName }, transaction));

        public async Task AddUserToRole(string userId, string roleName)
        {
            var roleId = await _roleRepository.GetFieldByNormalizedName(roleName.ToUpperInvariant(), role => role.Id);
            var userRole = new UserRole()
            {
                RoleId = roleId,
                UserId = userId
            };

            AddOperation((connection, transaction) => connection.ExecuteAsync(
                userRole.ToInsertQuery(),
                userRole, transaction));
        }
    }
}
