namespace WebdevPeriod3.Utilities
{
    /// <summary>
    /// A collection of methods to aid in creating SQL queries
    /// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// Creates a DELETE query
        /// </summary>
        /// <param name="tableName">The table from which to delete rows</param>
        /// <param name="keyName">The column by which to select the rows to be deleted</param>
        /// <param name="valueName">The name of the template value to put in the WHERE clause</param>
        /// <returns>
        /// A DELETE query for the table <paramref name="tableName"/>
        /// with a WHERE clause on the column <paramref name="keyName"/>
        /// which receives a templated value called <paramref name="valueName"/>
        /// </returns>
        public static string CreateDeleteQuery(string tableName, string keyName, string valueName) =>
            $"DELETE FROM {tableName} WHERE {keyName}=@{valueName}";
    }
}
