using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebdevPeriod3.Utilities
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts this object to an SQL key group
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <param name="values">The entity</param>
        /// <param name="keyExpression">
        /// An expression, the property in which,
        /// if provided, will be excluded from the key group
        /// </param>
        /// <returns>A key group, such as X=@X, Y=@Y, etc...</returns>
        public static string ToKeyGroup<E>(this E values, Expression<Func<E, object>> keyExpression = null) =>
            values.ToKeyGroup(keyExpression?.ExtractMemberName());

        /// <summary>
        /// Converts this object to an SQL key group
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <param name="_"></param>
        /// <param name="keyPropertyName">
        /// A property name, which, if provided, will be excluded from the key group
        /// </param>
        /// <returns>A key group, such as X=@X, Y=@Y, etc...</returns>
        private static string ToKeyGroup<E>(this E _, string keyPropertyName) =>
            string.Join(
                ',',
                typeof(E).GetProperties()
                .Where(property => keyPropertyName == null || property.Name != keyPropertyName)
                .Select(property => $"{property.Name}=@{property.Name}")
            );

        /// <summary>
        /// Converts this object to an UPDATE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property that will be used in the WHERE clause
        /// </param>
        /// <returns>
        /// An UPDATE query which runs on the table with the lower-case name of <typeparamref name="E"/> + 's'
        /// </returns>
        public static string ToUpdateQuery<E, K>(this E values, Expression<Func<E, K>> keyExpression) =>
            values.ToUpdateQuery(typeof(E).ToTableName(), keyExpression);

        /// <summary>
        /// Converts this object to an UPDATE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="tableName">The table's name</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property that will be used in the WHERE clause
        /// </param>
        /// <returns>
        /// An UPDATE query which runs on the table with the name <paramref name="tableName"/>
        /// </returns>
        public static string ToUpdateQuery<E, K>(this E values, string tableName, Expression<Func<E, K>> keyExpression)
        {
            var keyPropertyName = keyExpression.ExtractMemberName();

            return $"UPDATE {tableName} SET {values.ToKeyGroup(keyPropertyName)} WHERE {keyPropertyName}=@{keyPropertyName};";
        }

        /// <summary>
        /// Converts this object to an INSERT query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <returns>An INSERT query for the table with the lower-case name of <typeparamref name="E"/> + 's'</returns>
        public static string ToInsertQuery<E>(this E values) =>
            values.ToInsertQuery(typeof(E).ToTableName());

        /// <summary>
        /// Converts this object to an INSERT query
        /// </summary>
        /// <typeparam name="V">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="tableName">The table's name</param>
        /// <returns>An INSERT query for the table with the name <paramref name="tableName"/></returns>
        public static string ToInsertQuery<V>(this V values, string tableName) =>
            $"INSERT INTO {tableName} SET {values.ToKeyGroup()};";

        /// <summary>
        /// Converts this object to a DELETE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property to be used in the WHERE clause
        /// </param>
        /// <returns>A DELETE query for the table with the lower-case name of <typeparamref name="E"/> + 's'</returns>
        public static string ToDeleteQuery<E, K>(this E values, Expression<Func<E, K>> keyExpression)
            => values.ToDeleteQuery(typeof(E).ToTableName(), keyExpression);

        /// <summary>
        /// Converts this object to a DELETE query
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <param name="_">The values to be used in the query</param>
        /// <param name="tableName">A table's name</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property to be used in the WHERE clause
        /// </param>
        /// <returns>A DELETE query for the table with the name <paramref name="tableName"/></returns>
        public static string ToDeleteQuery<E, K>(this E _, string tableName, Expression<Func<E, K>> keyExpression) =>
            keyExpression.ToDeleteQuery(tableName);
    }
}
