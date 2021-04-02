using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Services
{
    public class CommentRepository : TransactionRepositoryBase
    {
        private static readonly Expression<Func<Comment, string>> ID_SELECTOR = comment => comment.Id;
        private static readonly Expression<Func<Comment, string>> PRODUCT_ID_SELECTOR = comment => comment.ProductId;
        private static readonly Expression<Func<Comment, string>> PARENT_ID_SELECTOR = comment => comment.ParentId;
        private static readonly Expression<Func<Comment, string>> CONTENT_SELECTOR = comment => comment.Content;
        private static readonly Expression<Func<Comment, bool>> FLAGGED_SELECTOR = comment => comment.Flagged;
        private static readonly Expression<Func<Comment, string>> POSTER_ID_SELECTOR = comment => comment.PosterId;
        private static readonly Expression<Func<User, string>> USER_ID_SELECTOR = user => user.Id;
        private static readonly Expression<Func<User, string>> USER_NAME_SELECTOR = user => user.UserName;

        public CommentRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration)
            : base(dapperTransactionService, configuration) { }

        public void AddComment(Comment comment)
        {
            if (comment.Id == null)
                comment.Id = Guid.NewGuid().ToString("N");

            AddOperation((connection, transaction) => connection.ExecuteAsync(comment.ToInsertQuery(), comment, transaction));
        }

        public Task<IEnumerable<Comment>> GetTopLevelComments(string productId) =>
            WithConnection(connection => connection.QueryAsync<Comment>(
                typeof(Comment).ToSelectQuery() + " " +
                PRODUCT_ID_SELECTOR.ToWhereClause(nameof(productId)) +
                $" AND {PARENT_ID_SELECTOR.ToColumnName()} IS NULL" +
                $" AND {FLAGGED_SELECTOR.ToColumnName()} IS FALSE;",
                new { productId }));

        public Task<IEnumerable<Comment>> GetReplies(string parentId) =>
            WithConnection(connection => connection.QueryAsync<Comment>(
                typeof(Comment).ToSelectQuery() + " " +
                PARENT_ID_SELECTOR.ToWhereClause(nameof(parentId)) +
                $" AND {FLAGGED_SELECTOR.ToColumnName()} IS FALSE;",
                new { parentId }));

        public Task<IEnumerable<Models.HydratedComment>> GetComments(string productId) =>
            WithConnection(connection => connection.QueryAsync<Models.HydratedComment>(
                $"SELECT" +
                $" {ID_SELECTOR.ToColumnName()}," +
                $" {CONTENT_SELECTOR.ToColumnName()}," +
                $" {USER_NAME_SELECTOR.ToColumnName()} AS {nameof(Models.HydratedComment.PosterName)}" +
                $" FROM {typeof(Comment).ToTableName()}" +
                $" INNER {POSTER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)}" +
                $" {PRODUCT_ID_SELECTOR.ToWhereClause(nameof(productId))}" +
                $" AND {FLAGGED_SELECTOR.ToColumnName()} IS FALSE;",
                new { productId }));

        public void FlagComment(string commentId) =>
            AddOperation((connection, transaction) => connection.ExecuteAsync(
                $"UPDATE {typeof(Comment).ToTableName()}" +
                $" SET {FLAGGED_SELECTOR.ToColumnName()} = 1" +
                $" {ID_SELECTOR.ToWhereClause(nameof(commentId))};",
                new { commentId }));
    }
}
