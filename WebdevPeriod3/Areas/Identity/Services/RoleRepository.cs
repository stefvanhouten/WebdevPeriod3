using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    public class RoleRepository : BaseRepository
    {
        private static readonly Expression<Func<Role, string>> ID_SELECTOR = role => role.Id;
        private static readonly Expression<Func<Role, string>> NORMALIZED_NAME_SELECTOR = role => role.NormalizedName;

        public RoleRepository(IConfiguration configuration): base(configuration) { }

        public async Task Add(Role role)
        {
            if (role.Id == null)
                role.Id = Guid.NewGuid().ToString("N");

            await WithConnection(
                connection => connection.ExecuteAsync(role.ToInsertQuery(), role));
        }

        public async Task Delete(Role role)
        {
            await WithConnection(
                connection => connection.ExecuteAsync(role.ToDeleteQuery(ID_SELECTOR)));
        }

        public Task<Role> FindById(string id) =>
            WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<Role>(
                    SqlHelper.CreateSelectWhereQuery(ID_SELECTOR, nameof(id)),
                    new { id }));

        public Task<Role> FindByNormalizedName(string normalizedName) =>
            WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<Role>(
                    SqlHelper.CreateSelectWhereQuery(NORMALIZED_NAME_SELECTOR, nameof(normalizedName)),
                    new { normalizedName }));

        public Task<T> GetFieldById<T>(string id, Expression<Func<Role, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    SqlHelper.CreateSelectWhereQuery(expression, ID_SELECTOR, nameof(id)),
                    new { id }));

        public Task<T> GetFieldByNormalizedName<T>(string normalizedName, Expression<Func<Role, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteScalarAsync<T>(
                    SqlHelper.CreateSelectWhereQuery(expression, NORMALIZED_NAME_SELECTOR, nameof(normalizedName)),
                    new { normalizedName }));

        public Task UpdateFieldById<T>(string id, T value, Expression<Func<Role, T>> expression) =>
            WithConnection(
                connection => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} {ID_SELECTOR.ToWhereClause(nameof(id))};",
                    new { value, id }));

        public Task Update(Role role) =>
            WithConnection(
                connection => connection.ExecuteAsync(
                    role.ToUpdateQuery(ID_SELECTOR)));
    }
}
