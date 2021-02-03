using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        private static readonly Expression<Func<User, string>> ID_SELECTOR = user => user.Id;
        private static readonly Expression<Func<User, string>> NORMALIZED_USERNAME_SELECTOR = user => user.NormalizedUserName;

        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task Add(User user)
        {
            if (user.Id == null)
                user.Id = Guid.NewGuid().ToString("N");

            await WithConnection(
                connection => connection.ExecuteAsync(
                    user.ToInsertQuery(), user));
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
            await WithConnection(
                connection => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} {ID_SELECTOR.ToWhereClause(nameof(id))};",
                    new { id, value }));
        }

        public async Task Update(User user)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(
                    user.ToUpdateQuery(ID_SELECTOR), user));
        }

        public async Task Delete(User user)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(user.ToDeleteQuery(ID_SELECTOR), user));
        }
    }
}
