using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Services;

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

        public Task<T> GetFieldById<T>(string id, Expression<Func<User, T>> expression)
        {
            var memberName = ExtractMemberName(expression);

            return WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<T>(
                    $"SELECT {memberName} FROM users WHERE Id=@id;",
                    new { id }));
        }

        public Task<T> GetFieldByNormalizedUserName<T>(string normalizedUserName, Expression<Func<User, T>> expression)
        {
            var memberName = ExtractMemberName(expression);

            return WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<T>(
                    $"SELECT {memberName} FROM users WHERE NormalizedUserName=@normalizedUserName;",
                    new { normalizedUserName }));
        }

        private string ExtractMemberName<T>(Expression<Func<User, T>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("The provided expression has to be a member access expression.");

            var body = expression.Body as MemberExpression;

            if (body.Expression.Type != typeof(User))
                throw new ArgumentException("The provided expression has to be performed on a user.");

            return body.Member.Name;
        }
    }
}
