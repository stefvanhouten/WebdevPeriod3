using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    public class UserRepository : BaseRepository
    {
        public class DuplicateUserNameException : ArgumentException
        {
            public DuplicateUserNameException() : base("A user with the provided user name already exists.") { }
        }

        private static readonly Expression<Func<User, string>> ID_SELECTOR = user => user.Id;
        private static readonly Expression<Func<User, string>> NORMALIZED_USERNAME_SELECTOR = user => user.NormalizedUserName;

        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Add(User user)
        {
            try
            {
                if (user.Id == null)
                    user.Id = Guid.NewGuid().ToString("N");

                await WithConnection(
                    connection => connection.ExecuteAsync(
                        user.ToInsertQuery(), user));
            }
            catch (MySqlException exception)
            {
                switch ((MySqlErrorCode)exception.Number)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        throw new DuplicateUserNameException();
                    default:
                        throw exception;
                }
            }
        }

        public Task<IEnumerable<User>> GetAll() =>
            WithConnection(connection => connection.QueryAsync<User>(typeof(User).ToSelectQuery()));

        public Task<User> FindById(string id) =>
            WithConnection(
                connection => connection.QueryFirstOrDefaultAsync<User>(
                    SqlHelper.CreateSelectWhereQuery(ID_SELECTOR, nameof(id)),
                    new { id }));

        public Task<User> FindByNormalizedUserName(string normalizedUserName) =>
            WithConnection(
                connection => connection.QueryFirstOrDefaultAsync<User>(
                    SqlHelper.CreateSelectWhereQuery(NORMALIZED_USERNAME_SELECTOR, nameof(normalizedUserName)),
                    new { normalizedUserName }));

        public Task<T> GetFieldById<T>(string id, Expression<Func<User, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    SqlHelper.CreateSelectWhereQuery(expression, ID_SELECTOR, nameof(id)),
                    new { id }));

        public Task<T> GetFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    SqlHelper.CreateSelectWhereQuery(expression, NORMALIZED_USERNAME_SELECTOR, nameof(normalizedUserName)),
                    new { normalizedUserName }));

        public async Task UpdateFieldById<T>(string id, Expression<Func<User, T>> expression, T value)
        {
            try {
                await WithConnection(
                    connection => connection.ExecuteAsync(
                        $"{expression.ToUpdateClause(nameof(value))} {ID_SELECTOR.ToWhereClause(nameof(id))};",
                        new { id, value }));
            } catch (MySqlException exception)
            {
                switch ((MySqlErrorCode)exception.ErrorCode)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        throw new DuplicateUserNameException();
                    default:
                        throw exception;
                }
            }
        }

        public async Task UpdateFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression, T value)
        {
            try
            {
                await WithConnection(
                    connection => connection.ExecuteAsync(
                        $"{expression.ToUpdateClause(nameof(value))} {NORMALIZED_USERNAME_SELECTOR.ToWhereClause(nameof(normalizedUserName))};",
                        new { normalizedUserName, value }));
            } catch (MySqlException exception)
            {
                switch ((MySqlErrorCode)exception.ErrorCode)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        throw new DuplicateUserNameException();
                    default:
                        throw exception;
                }
            }
        }

        public async Task Update(User user)
        {
            try
            {
                await WithConnection(
                    connection => connection.ExecuteAsync(
                        user.ToUpdateQuery(ID_SELECTOR), user));
            } catch (MySqlException exception)
            {
                switch ((MySqlErrorCode)exception.ErrorCode)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        throw new DuplicateUserNameException();
                    default:
                        throw exception;
                }
            }
        }

        public async Task Delete(User user)
        {
            if (user.Id != null)
                await WithConnection(
                    connection => connection.ExecuteAsync(user.ToDeleteQuery(ID_SELECTOR), user));
            else if (user.NormalizedUserName != null)
                await WithConnection(
                    connection => connection.ExecuteAsync(user.ToDeleteQuery(NORMALIZED_USERNAME_SELECTOR), user));
            else
            {
                user.NormalizedUserName = user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException("The user has to have a non-null ID, normalized user name or user name.");

                await WithConnection(
                    connection => connection.ExecuteAsync(user.ToDeleteQuery(NORMALIZED_USERNAME_SELECTOR), user));
            }
        }
    }
}
