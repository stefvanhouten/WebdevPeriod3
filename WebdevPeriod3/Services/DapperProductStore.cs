using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Utilities;

namespace WebdevPeriod3.Services
{
    public class DapperProductStore
    {
        private readonly DapperTransactionService _dapperTransactionService;
        private readonly ProductRepository _productRepository;
        public DapperProductStore(ProductRepository productRepository, DapperTransactionService dapperTransactionService)
        {
            _productRepository = productRepository;
            _dapperTransactionService = dapperTransactionService;
        }

        public async Task AddProductAsync(Product product)
        {
            _productRepository.AddProduct(product);

            await _dapperTransactionService.RunOperations();
        }

        public async Task AddSubProductAsync(ProductRelation productRelation)
        {
            _productRepository.AddSubProduct(productRelation);

            await _dapperTransactionService.RunOperations();
        }

    }
}