using System;
using System.Linq.Expressions;

namespace WebdevPeriod3.Utilities
{
    /// <summary>
    /// A collection of methods to aid in creating SQL queries
    /// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// Creates a WHERE clause
        /// </summary>
        /// <param name="keyName">The column's name</param>
        /// <param name="valueName">The template value's name</param>
        /// <returns>A WHERE clause, such as "WHERE X=@Y"</returns>
        public static string CreateWhereClause(string keyName, string valueName) =>
            $"WHERE {keyName}=@{valueName}";

        /// <summary>
        /// Creates a SELECT ... WHERE query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <param name="keyName">The name of the column for the WHERE clause</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>
        /// A SELECT query for the table <paramref name="tableName"/>
        /// with a WHERE clause on the column <paramref name="keyName"/>
        /// which receives a template value called <paramref name="valueName"/>
        /// </returns>
        public static string CreateSelectWhereQuery(string tableName, string keyName, string valueName) =>
            CreateSelectWhereQuery(tableName, "*", keyName, valueName);

        /// <summary>
        /// Creates a SELECT ... WHERE query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <param name="columns">The names of the columns to select</param>
        /// <param name="keyName">The name of the column for the WHERE clause</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>
        /// A SELECT query for the table <paramref name="tableName"/>
        /// which selects the columns in <paramref name="columns"/>
        /// with a WHERE clause on the column <paramref name="keyName"/>
        /// which receives a template value called <paramref name="valueName"/>
        /// </returns>
        public static string CreateSelectWhereQuery(string tableName, string[] columns, string keyName, string valueName) =>
            CreateSelectWhereQuery(tableName, string.Join(',', columns), keyName, valueName);

        /// <summary>
        /// Creates a SELECT ... WHERE query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <param name="columns">A string to insert between SELECT and FROM</param>
        /// <param name="keyName">The name of the column for the WHERE clause</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>
        /// A SELECT query for the table <paramref name="tableName"/>
        /// which selects the columns in <paramref name="columns"/>
        /// with a WHERE clause on the column <paramref name="keyName"/>
        /// which receives a template value called <paramref name="valueName"/>
        /// </returns>
        public static string CreateSelectWhereQuery(string tableName, string columns, string keyName, string valueName) =>
            $"{CreateSelectQuery(tableName, columns)} WHERE {keyName}=@{valueName}";

        /// <summary>
        /// Creates a SELECT query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <returns>A SELECT query that selects all cells in the table <paramref name="tableName"/></returns>
        public static string CreateSelectQuery(string tableName) =>
            CreateSelectQuery(tableName, "*");

        /// <summary>
        /// Creates a SELECT query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <param name="columns">The columns to select</param>
        /// <returns>
        /// A SELECT query that selects the columns <paramref name="columns"/>
        /// from the table <paramref name="tableName"/>
        /// </returns>
        public static string CreateSelectQuery(string tableName, string[] columns) =>
            CreateSelectQuery(tableName, string.Join(',', columns));

        /// <summary>
        /// Creates a SELECT query
        /// </summary>
        /// <param name="tableName">The table's name</param>
        /// <param name="columns">A string to insert between SELECT and FROM</param>
        /// <returns>
        /// A SELECT query that selects the columns <paramref name="columns"/>
        /// from the table <paramref name="tableName"/>
        /// </returns>
        public static string CreateSelectQuery(string tableName, string columns) =>
            $"SELECT {columns} FROM {tableName}";

        /// <summary>
        /// Creates a DELETE query
        /// </summary>
        /// <param name="tableName">The table from which to delete rows</param>
        /// <param name="keyName">The column by which to select the rows to be deleted</param>
        /// <param name="valueName">The name of the template value to put in the WHERE clause</param>
        /// <returns>
        /// A DELETE query for the table <paramref name="tableName"/>
        /// with a WHERE clause on the column <paramref name="keyName"/>
        /// which receives a template value called <paramref name="valueName"/>
        /// </returns>
        public static string CreateDeleteQuery(string tableName, string keyName, string valueName) =>
            $"DELETE FROM {tableName} {CreateWhereClause(keyName, valueName)}";

        /// <summary>
        /// Creates a SELECT ... WHERE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="keyExpression">A member expression containing the property by which the WHERE clause filters</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>A SELECT ... WHERE query that selects from the lower-case name of <typeparamref name="E"/> + 's'</returns>
        public static string CreateSelectWhereQuery<E, K>(
            Expression<Func<E, K>> keyExpression,
            string valueName) =>
            CreateSelectWhereQuery(typeof(E).ToTableName(), keyExpression, valueName);

        /// <summary>
        /// Creates a SELECT ... WHERE query for the table <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="tableName">The table's name</param>
        /// <param name="keyExpression">A member expression containing the property by which the WHERE clause filters</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>A SELECT ... WHERE query that selects from the table <paramref name="tableName"/></returns>
        public static string CreateSelectWhereQuery<E, K>(
            string tableName,
            Expression<Func<E, K>> keyExpression,
            string valueName) =>
            CreateSelectWhereQuery(tableName, keyExpression.ExtractMemberName(), valueName);

        /// <summary>
        /// Creates a SELECT ... WHERE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <typeparam name="V">The column's type</typeparam>
        /// <param name="columnExpression">A member expression containing the property which will be selected</param>
        /// <param name="keyExpression">A member expression containing the property by which the WHERE clause filters</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>
        /// A SELECT ... WHERE query that selects the property in <paramref name="columnExpression"/>
        /// from the table with the lower-case name of <typeparamref name="E"/> + 's'
        /// </returns>
        public static string CreateSelectWhereQuery<E, K, V>(
            Expression<Func<E, V>> columnExpression,
            Expression<Func<E, K>> keyExpression,
            string valueName) =>
            CreateSelectWhereQuery(typeof(E).ToTableName(), columnExpression, keyExpression, valueName);

        /// <summary>
        /// Creates a SELECT ... WHERE query for the table <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <typeparam name="V">The column's type</typeparam>
        /// <param name="tableName">The table's name</param>
        /// <param name="columnExpression">A member expression containing the property which will be selected</param>
        /// <param name="keyExpression">A member expression containing the property by which the WHERE clause filters</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>
        /// A SELECT ... WHERE query that selects the property in <paramref name="columnExpression"/>
        /// from the table <paramref name="tableName"/>
        /// </returns>
        public static string CreateSelectWhereQuery<E, K, V>(
            string tableName,
            Expression<Func<E, V>> columnExpression,
            Expression<Func<E, K>> keyExpression,
            string valueName) =>
            CreateSelectWhereQuery(
                tableName,
                columnExpression.ExtractMemberName(),
                keyExpression.ExtractMemberName(),
                valueName);
    }
}
