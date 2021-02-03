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
            WithConnection(connection => connection.QueryAsync<User>("SELECT * FROM users;"));

        public Task<User> FindById(string id) =>
            WithConnection(
                connection => connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM users WHERE Id=@id",
                    new { id }));

        public Task<User> FindByNormalizedUserName(string normalizedUserName) =>
            WithConnection(
                connection => connection.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM users WHERE NormalizedUserName=@normalizedUserName",
                    new { normalizedUserName }));

        public Task<T> GetFieldById<T>(string id, Expression<Func<User, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    $"{expression.ToSelectClause()} WHERE Id=@id;",
                    new { id }));

        public Task<T> GetFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    $"{expression.ToSelectClause()} WHERE NormalizedUserName=@normalizedUserName;",
                    new { normalizedUserName }));

        public async Task UpdateFieldById<T>(string id, Expression<Func<User, T>> expression, T value)
        {
            var values = new { id, value };

            await WithConnection(
                connection => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} WHERE Id=@id;", new { id, value }));
        }

        public async Task Update(User user)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(
                    user.ToUpdateQuery(user => user.Id), user));
        }

        public async Task Delete(User user)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(user.ToDeleteQuery(user => user.Id), user));
        }
    }
}
