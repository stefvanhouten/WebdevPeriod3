using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Services
{
    public class ProductRepository : TransactionRepositoryBase
    {
        private static readonly Expression<Func<Product, string>> POSTER_ID_SELECTOR = product => product.PosterId;
        private static readonly Expression<Func<Product, string>> ID_SELECTOR = product => product.Id;
        private static readonly Expression<Func<Product, string>> NAME_SELECTOR = product => product.Name;
        private static readonly Expression<Func<Product, byte[]>> IMAGE_SELECTOR = product => product.Image;
        private static readonly Expression<Func<ProductRelation, string>> PRODUCT_RELATION_SUB_PRODUCT_ID_SELECTOR = relation => relation.SubProductId;
        private static readonly Expression<Func<ProductRelation, string>> PRODUCT_RELATION_PRODUCT_ID_SELECTOR = relation => relation.ProductId;
        private static readonly Expression<Func<Comment, string>> COMMENT_ID_SELECTOR = comment => comment.Id;
        private static readonly Expression<Func<Comment, string>> COMMENT_PRODUCT_ID_SELECTOR = comment => comment.ProductId;
        private static readonly Expression<Func<Comment, string>> COMMENT_PARENT_ID_SELECTOR = comment => comment.ParentId;
        private static readonly Expression<Func<Comment, string>> COMMENT_CONTENT_SELECTOR = comment => comment.Content;
        private static readonly Expression<Func<Comment, bool>> COMMENT_FLAGGED_SELECTOR = comment => comment.Flagged;
        private static readonly Expression<Func<Comment, string>> COMMENT_POSTER_ID_SELECTOR = comment => comment.PosterId;
        private static readonly Expression<Func<User, string>> USER_ID_SELECTOR = user => user.Id;
        private static readonly Expression<Func<User, string>> USER_NAME_SELECTOR = user => user.UserName;

        public ProductRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(dapperTransactionService, configuration)
        {
        }

        public void AddProduct(Product product)
        {
            if (product.Id == null)
                product.Id = Guid.NewGuid().ToString("N");

            AddOperation(
               (connection, transaction) => connection.ExecuteAsync(product.ToInsertQuery(), product, transaction));
        }
        public void AddSubProduct(ProductRelation productRelation)
        {
            AddOperation(
               (connection, transaction) => connection.ExecuteAsync(productRelation.ToInsertQuery(), productRelation, transaction));
        }
        public void Delete(Product product)
        {
            AddOperation(
                (connection, transaction) => connection.ExecuteAsync(product.ToDeleteQuery(ID_SELECTOR), product, transaction));
        }

        public Task<IEnumerable<Product>> GetAllProducts() =>
            WithConnection(connection => connection.QueryAsync<Product>(SqlHelper.CreateSelectQuery("products")));

        public Task<IEnumerable<Product>> GetAllProductsInCatalog()
        {
            var parameters = new { ShowInCatalog = true };

            return WithConnection(connection => connection.QueryAsync<Product>(
                SqlHelper.CreateSelectWhereQuery((Product product) => product.ShowInCatalog, nameof(parameters.ShowInCatalog)),
                parameters));
        }

        public Task<IEnumerable<Product>> FindProductsByPosterId(string posterId) =>
            WithConnection(
               connection => connection.QueryAsync<Product>(
                   SqlHelper.CreateSelectWhereQuery(POSTER_ID_SELECTOR, nameof(posterId)),
                   new { posterId }));

        public Task<IEnumerable<Product>> FindProductsBySearchTerm(string searchTerm) =>
            WithConnection(
                connection => connection.QueryAsync<Product>(
                    $"{typeof(Product).ToSelectQuery()} " +
                    $"WHERE {NAME_SELECTOR.ToColumnName() } " +
                    $"LIKE '%{searchTerm}%';",
                    new { searchTerm })
                );

        public Task<IEnumerable<Product>> FindProductsWithIds(IEnumerable<string> ids) =>
            WithConnection(
                connection => connection.QueryAsync<Product>(
                    $"{typeof(Product).ToSelectQuery()} " +
                    $"WHERE {ID_SELECTOR.ToColumnName()} " +
                    $"IN @{nameof(ids)};",
                    new { ids = ids.ToArray() }));

        public Task<(Product product, IEnumerable<Product> subProducts, IEnumerable<Models.HydratedComment> comments)?> FindProductById(string id) =>
            WithConnection<SqlMapper.GridReader, (Product product, IEnumerable<Product> subProducts, IEnumerable<Models.HydratedComment> comments)?>(
                connection => connection.QueryMultipleAsync(
                    $"{typeof(Product).ToSelectQuery()} " +
                    $"{ID_SELECTOR.ToWhereClause(nameof(id))};" +
                    $"{SqlHelper.CreateSelectQuery(typeof(ProductRelation).ToTableName(), "subProducts.*")} " +
                    $"{PRODUCT_RELATION_SUB_PRODUCT_ID_SELECTOR.ToJoinClause(ID_SELECTOR, rightTableAlias: "subProducts")} " +
                    $"{PRODUCT_RELATION_PRODUCT_ID_SELECTOR.ToWhereClause(nameof(id))};" +
                    $"SELECT" +
                    $" {COMMENT_ID_SELECTOR.ToColumnName()}," +
                    $" {COMMENT_PARENT_ID_SELECTOR.ToColumnName()}," +
                    $" {COMMENT_CONTENT_SELECTOR.ToColumnName()}," +
                    $" {USER_NAME_SELECTOR.ToColumnName()} AS {nameof(Models.HydratedComment.PosterName)}" +
                    $" FROM {typeof(Comment).ToTableName()}" +
                    $" INNER {COMMENT_POSTER_ID_SELECTOR.ToJoinClause(USER_ID_SELECTOR)}" +
                    $" {COMMENT_PRODUCT_ID_SELECTOR.ToWhereClause(nameof(id))}" +
                    $" AND {COMMENT_FLAGGED_SELECTOR.ToColumnName()} IS FALSE;",
                    new { id }),
                async result =>
                {
                    try
                    {
                        return new(await result.ReadSingleAsync<Product>(), await result.ReadAsync<Product>(), await result.ReadAsync<Models.HydratedComment>());
                    }
                    catch (InvalidOperationException)
                    {
                        return null;
                    }
                });

        public Task<byte[]> FindImageByProductId(string productId) =>
            WithConnection(
                connection => connection.QuerySingleOrDefaultAsync<byte[]>(
                    $"{IMAGE_SELECTOR.ToSelectClause()} {ID_SELECTOR.ToWhereClause(nameof(productId))};",
                    new { productId }
                    ));
    }
}
