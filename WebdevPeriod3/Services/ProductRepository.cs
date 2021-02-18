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
		private readonly DapperTransactionService _dapperTransactionService;

		private static readonly Expression<Func<Product, string>> POSTER_ID_SELECTOR = product => product.PosterId;
		private static readonly Expression<Func<Product, string>> ID_SELECTOR = product => product.Id;
		public ProductRepository(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(dapperTransactionService, configuration)
		{
		}

		public void Add(Product product)
		{
			if (product.Id == null)
				product.Id = Guid.NewGuid().ToString("N");

			AddOperation(
			   (connection, transaction) => connection.ExecuteAsync(product.ToInsertQuery(), product, transaction));
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
	}
}
