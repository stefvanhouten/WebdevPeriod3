using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Services
{
    public class ProductRepository : TransactionRepositoryBase
    {
        private static readonly Expression<Func<Product, string>> POSTER_ID_SELECTOR = product => product.PosterId;
        private static readonly Expression<Func<Product, string>> ID_SELECTOR = product => product.Id;
        private static readonly Expression<Func<Product, string>> NAME_SELECTOR = product => product.Name;
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
    }
}
