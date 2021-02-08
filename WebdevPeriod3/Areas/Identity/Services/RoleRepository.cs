using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Areas.Identity.Services
{
    public class RoleRepository : TransactionRepositoryBase
    {
        public class DuplicateRoleNameException : ArgumentException
        {
            public DuplicateRoleNameException() : base("Ä role with the provided role name already exists.") { }
        }

        private static readonly Expression<Func<Role, string>> ID_SELECTOR = role => role.Id;
        private static readonly Expression<Func<Role, string>> NORMALIZED_NAME_SELECTOR = role => role.NormalizedName;

        public RoleRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(dapperTransactionService, configuration) { }

        public void Add(Role role)
        {
            if (role.Id == null)
                role.Id = Guid.NewGuid().ToString("N");

            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(role.ToInsertQuery(), role, transaction));
        }

        public void Delete(Role role)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(role.ToDeleteQuery(ID_SELECTOR), role, transaction));
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

        public void UpdateFieldById<T>(string id, T value, Expression<Func<Role, T>> expression)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} {ID_SELECTOR.ToWhereClause(nameof(id))};",
                    new { value, id },
                    transaction));
        }

        public void Update(Role role)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(
                    role.ToUpdateQuery(ID_SELECTOR), role, transaction));
        }
    }
}
