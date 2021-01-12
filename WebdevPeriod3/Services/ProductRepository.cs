﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;

namespace WebdevPeriod3.Services
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        private readonly IProductCommandText _commandText;

        public ProductRepository(IConfiguration configuration, IProductCommandText commandText) : base(configuration)
        {
            _commandText = commandText;

        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {

            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<Product>(_commandText.GetProducts);
                return query;
            });

        }

        public async ValueTask<Product> GetById(int id)
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryFirstOrDefaultAsync<Product>(_commandText.GetProductById, new { Id = id });
                return query;
            });

        }

        public async Task AddProduct(Product entity)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.AddProduct,
                    new { Name = entity.Name, Cost = entity.Cost, CreatedDate = entity.CreatedDate });
            });

        }
        public async Task UpdateProduct(Product entity, int id)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.UpdateProduct,
                    new { Name = entity.Name, Cost = entity.Cost, Id = id });
            });

        }

        public async Task RemoveProduct(int id)
        {

            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.RemoveProduct, new { Id = id });
            });

        }
    }
}
