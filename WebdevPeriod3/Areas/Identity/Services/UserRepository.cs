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
    public class UserRepository : TransactionRepositoryBase
    {
        public class DuplicateUserNameException : ArgumentException
        {
            public DuplicateUserNameException() : base("A user with the provided user name already exists.") { }
        }

        private static readonly Expression<Func<User, string>> ID_SELECTOR = user => user.Id;
        private static readonly Expression<Func<User, string>> NORMALIZED_USERNAME_SELECTOR = user => user.NormalizedUserName;

        public UserRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(dapperTransactionService, configuration) { }

        public void Add(User user)
        {
            if (user.Id == null)
                user.Id = Guid.NewGuid().ToString("N");

            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(
                    user.ToInsertQuery(), user, transaction));
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

        public void UpdateFieldById<T>(string id, Expression<Func<User, T>> expression, T value)
        {
            AddOperation(
                   (connection, transaction) => connection.ExecuteAsync(
                       $"{expression.ToUpdateClause(nameof(value))} {ID_SELECTOR.ToWhereClause(nameof(id))};",
                       new { id, value }, transaction));
        }

        public void UpdateFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression, T value)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(
                    $"{expression.ToUpdateClause(nameof(value))} {NORMALIZED_USERNAME_SELECTOR.ToWhereClause(nameof(normalizedUserName))};",
                    new { normalizedUserName, value }, transaction));
        }

        public void Update(User user)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(
                    user.ToUpdateQuery(ID_SELECTOR), user, transaction));
        }

        public void Delete(User user)
        {
            if (user.Id != null)
                AddOperation(
                    (connection, transaction) => connection.ExecuteAsync(user.ToDeleteQuery(ID_SELECTOR), user, transaction));
            else if (user.NormalizedUserName != null)
                AddOperation(
                    (connection, transaction) => connection.ExecuteAsync(user.ToDeleteQuery(NORMALIZED_USERNAME_SELECTOR), user, transaction));
            else
            {
                user.NormalizedUserName = user.UserName?.ToUpperInvariant()
                    ?? throw new ArgumentException("The user has to have a non-null ID, normalized user name or user name.");

                AddOperation(
                    (connection, transaction) => connection.ExecuteAsync(user.ToDeleteQuery(NORMALIZED_USERNAME_SELECTOR), user, transaction));
            }
        }
    }
}
