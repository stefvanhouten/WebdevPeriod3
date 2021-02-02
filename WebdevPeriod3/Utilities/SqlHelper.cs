using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Utilities
{
    /// <summary>
    /// A helper class for building queries
    /// </summary>
    /// <typeparam name="T">The type of the table's entities</typeparam>
    public class SqlHelper<T>
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        protected readonly string _tableName;

        /// <summary>
        /// Creates a helper for the table with the name <paramref name="tableName"/>
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        public SqlHelper(string tableName)
        {
            _tableName = tableName;
        }
    }
}
