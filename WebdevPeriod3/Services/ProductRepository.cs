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

        public Task<(Product product, IEnumerable<Product> subProducts)?> FindProductById(string id) =>
            WithConnection<SqlMapper.GridReader, (Product product, IEnumerable<Product> subProducts)?>(
                connection => connection.QueryMultipleAsync(
                    $"{typeof(Product).ToSelectQuery()} " +
                    $"{ID_SELECTOR.ToWhereClause(nameof(id))};" +
                    $"{SqlHelper.CreateSelectQuery(typeof(ProductRelation).ToTableName(), "subProducts.*")} " +
                    $"{PRODUCT_RELATION_SUB_PRODUCT_ID_SELECTOR.ToJoinClause(ID_SELECTOR, rightTableAlias: "subProducts")} " +
                    $"{PRODUCT_RELATION_PRODUCT_ID_SELECTOR.ToWhereClause(nameof(id))};",
                    new { id }),
                async result =>
                {
                    try
                    {
                        return new(await result.ReadSingleAsync<Product>(), await result.ReadAsync<Product>());
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
