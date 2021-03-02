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
    public class ProductRepository : TransactionRepositoryBase
    {
        private static readonly Expression<Func<Product, string>> POSTER_ID_SELECTOR = product => product.PosterId;
        public ProductRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(dapperTransactionService, configuration)
        {
        }

        public Task<IEnumerable<Product>> FindProductsByPosterId(string posterId) =>
            WithConnection(
               connection => connection.QueryAsync<Product>(
                   SqlHelper.CreateSelectWhereQuery(POSTER_ID_SELECTOR, nameof(posterId)),
                   new { posterId }));
    }
}
