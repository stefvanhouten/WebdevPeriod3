using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace WebdevPeriod3.Services
{
    public class DapperTransactionService : BaseRepository
    {
        private Queue<Func<IDbConnection, IDbTransaction, Task>> _operations = new Queue<Func<IDbConnection, IDbTransaction, Task>>();

        public DapperTransactionService(IConfiguration configuration) : base(configuration) { }

        public void AddOperation(Func<IDbConnection, IDbTransaction, Task> operation)
        {
            lock (_operations)
                _operations.Enqueue(operation);
        }

        public async Task RunOperations(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Queue<Func<IDbConnection, IDbTransaction, Task>> operations; 

            lock (_operations)
            {
                operations = _operations;

                _operations = new Queue<Func<IDbConnection, IDbTransaction, Task>>();
            }

            await WithConnection(async connection =>
            {
                using var transaction = connection.BeginTransaction();

                while (operations.Count > 0)
                {
                    await operations.Dequeue()(connection, transaction);
                }

                transaction.Commit();
            });
        }
    }
}
