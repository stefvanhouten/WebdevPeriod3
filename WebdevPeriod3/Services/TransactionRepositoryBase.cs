using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Services
{
    public class TransactionRepositoryBase : BaseRepository
    {
        private readonly DapperTransactionService _dapperTransactionService;

        public TransactionRepositoryBase(DapperTransactionService dapperTransactionService, IConfiguration configuration) : base(configuration)
        {
            _dapperTransactionService = dapperTransactionService;
        }

        protected void AddOperation(Func<IDbConnection, IDbTransaction, Task> operation) {
            _dapperTransactionService.AddOperation(operation);
        }
    }
}
