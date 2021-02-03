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
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The entity</param>
        /// <param name="keyExpression">
        /// An expression, the property in which,
        /// if provided, will be excluded from the key group
        /// </param>
        /// <returns>A key group, such as X=@X, Y=@Y, etc...</returns>
        public static string ToKeyGroup<T>(this T values, Expression<Func<T, object>> keyExpression = null) =>
            values.ToKeyGroup(keyExpression?.ExtractMemberName());

        /// <summary>
        /// Converts this object to an SQL key group
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="_"></param>
        /// <param name="keyPropertyName">
        /// A property name, which, if provided, will be excluded from the key group
        /// </param>
        /// <returns>A key group, such as X=@X, Y=@Y, etc...</returns>
        private static string ToKeyGroup<T>(this T _, string keyPropertyName) =>
            string.Join(
                ',',
                typeof(T).GetProperties()
                .Where(property => keyPropertyName == null || property.Name != keyPropertyName)
                .Select(property => $"{property.Name}=@{property.Name}")
            );

        /// <summary>
        /// Converts this object to an UPDATE query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property that will be used in the WHERE clause
        /// </param>
        /// <returns>
        /// An UPDATE query which runs on the table with the lower-case name of <typeparamref name="T"/> + 's'
        /// </returns>
        public static string ToUpdateQuery<T>(this T values, Expression<Func<T, object>> keyExpression) =>
            values.ToUpdateQuery(typeof(T).ToTableName(), keyExpression);

        /// <summary>
        /// Converts this object to an UPDATE query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="tableName">The table's name</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property that will be used in the WHERE clause
        /// </param>
        /// <returns>
        /// An UPDATE query which runs on the table with the name <paramref name="tableName"/>
        /// </returns>
        public static string ToUpdateQuery<T>(this T values, string tableName, Expression<Func<T, object>> keyExpression)
        {
            var keyPropertyName = keyExpression.ExtractMemberName();

            return $"UPDATE {tableName} SET {values.ToKeyGroup(keyPropertyName)} WHERE {keyPropertyName}=@{keyPropertyName};";
        }

        /// <summary>
        /// Converts this object to an INSERT query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <returns>An INSERT query for the table with the lower-case name of <typeparamref name="T"/> + 's'</returns>
        public static string ToInsertQuery<T>(this T values) =>
            values.ToInsertQuery(typeof(T).ToTableName());

        /// <summary>
        /// Converts this object to an INSERT query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="tableName">The table's name</param>
        /// <returns>An INSERT query for the table with the name <paramref name="tableName"/></returns>
        public static string ToInsertQuery<T>(this T values, string tableName) =>
            $"INSERT INTO {tableName} SET {values.ToKeyGroup()};";

        /// <summary>
        /// Converts this object to a DELETE query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="values">The values to be used in the query</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property to be used in the WHERE clause
        /// </param>
        /// <returns>A DELETE query for the table with the lower-case name of <typeparamref name="T"/> + 's'</returns>
        public static string ToDeleteQuery<T>(this T values, Expression<Func<T, object>> keyExpression)
            => values.ToDeleteQuery(typeof(T).ToTableName(), keyExpression);

        /// <summary>
        /// Converts this object to a DELETE query
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <param name="_">The values to be used in the query</param>
        /// <param name="tableName">A table's name</param>
        /// <param name="keyExpression">
        /// A member access expression that contains the property to be used in the WHERE clause
        /// </param>
        /// <returns>A DELETE query for the table with the name <paramref name="tableName"/></returns>
        public static string ToDeleteQuery<T>(this T _, string tableName, Expression<Func<T, object>> keyExpression) =>
            keyExpression.ToDeleteQuery(tableName);
    }
}
