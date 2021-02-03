using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebdevPeriod3.Utilities
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the member name from an expression
        /// </summary>
        /// <typeparam name="T">The type of the object from which the member is accessed</typeparam>
        /// <typeparam name="U">The member's type</typeparam>
        /// <param name="expression">The expression from which to get the member name</param>
        /// <returns>The member name accessed in <paramref name="expression"/></returns>
        public static string ExtractMemberName<T, U>(this Expression<Func<T, U>> expression)
        {
            if (expression.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException("The provided expression has to be a member access expression.");

            var body = expression.Body as MemberExpression;

            if (body.Expression.Type != typeof(T))
                throw new ArgumentException($"The provided expression has to be performed on a {typeof(T).Name}.");

            return body.Member.Name;
        }

        /// <summary>
        /// Converts an expression to a select clause for a table with the name of <typeparamref name="E"/> + 's'
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<E, V>(this Expression<Func<E, V>> expression) =>
            expression.ToSelectClause(typeof(E).ToTableName());

        /// <summary>
        /// Converts an expression to a select clause for a table with the name <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <param name="tableName">The table's name</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<E, V>(this Expression<Func<E, V>> expression, string tableName) =>
            $"SELECT {expression.ExtractMemberName()} FROM {tableName}";

        /// <summary>
        /// Converts an expression to an update clause for a table with the name of <typeparamref name="E"/> + 's'
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to an update clause</param>
        /// <param name="valueName">The name of the template value</param>
        /// <returns>An update clause</returns>
        public static string ToUpdateClause<E, V>(this Expression<Func<E, V>> expression, string valueName) =>
            expression.ToUpdateClause(typeof(E).ToTableName(), valueName);

        /// <summary>
        /// Converts an expression to an update clause for a table with the name <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to an update clause</param>
        /// <param name="tableName">The table's name</param>
        /// <param name="valueName">The name of the template value</param>
        /// <returns>An update clause</returns>
        public static string ToUpdateClause<E, V>(this Expression<Func<E, V>> expression, string tableName, string valueName) =>
            $"UPDATE {tableName} SET {expression.ExtractMemberName()}=@{valueName}";

        /// <summary>
        /// Converts an expression to a DELETE query for a table with the lower-case name of <typeparamref name="E"/> + 's'
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="expression">A member access expression to select the column which is used in the WHERE clause</param>
        /// <returns>A DELETE query</returns>
        public static string ToDeleteQuery<E, K>(this Expression<Func<E, K>> expression) =>
            expression.ToDeleteQuery(typeof(E).ToTableName());

        /// <summary>
        /// Converts an expression to a DELETE query for the table <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="expression">A member access expression to select the column which is used in the WHERE clause</param>
        /// <param name="tableName">The table from which to delete rows</param>
        /// <returns>A DELETE query</returns>
        public static string ToDeleteQuery<E, K>(this Expression<Func<E, K>> expression, string tableName)
        {
            var keyName = expression.ExtractMemberName();

            return SqlHelper.CreateDeleteQuery(tableName, keyName, keyName);
        }

        /// <summary>
        /// Coverts an expression to a WHERE clause
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="expression">A member access expression to select the column which is used in the WHERE clause</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>Returns a WHERE clause such as "WHERE X=@Y"</returns>
        public static string ToWhereClause<E, K>(this Expression<Func<E, K>> expression, string valueName)
        {
            return SqlHelper.CreateWhereClause(expression.ExtractMemberName(), valueName);
        }
    }
}
