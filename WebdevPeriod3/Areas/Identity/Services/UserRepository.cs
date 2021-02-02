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
                connection => connection.QuerySingleOrDefaultAsync<T>(
                    $"{expression.ToSelectClause()} WHERE Id=@id;",
                    new { id }));

        public Task<T> GetFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression) =>
            WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<T>(
                    $"{expression.ToSelectClause()} WHERE NormalizedUserName=@normalizedUserName;",
                    new { normalizedUserName }));

        public async Task UpdateFieldById<T>(string id, Expression<Func<User, T>> expression, T value)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} WHERE Id=@id;", new { id, value }));
        }
    }
}
