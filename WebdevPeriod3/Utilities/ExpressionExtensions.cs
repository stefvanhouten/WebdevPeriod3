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
        /// Converts an expression to a column name
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <typeparam name="U">The column's type</typeparam>
        /// <param name="expression">The expression to convert to a column name</param>
        /// <returns>A column name such as "TableName.ColumnName"</returns>
        public static string ToColumnName<T, U>(this Expression<Func<T, U>> expression) =>
            expression.ToColumnName(typeof(T).ToTableName());

        /// <summary>
        /// Converts an expression to a column name
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <typeparam name="U">The column's type</typeparam>
        /// <param name="expression">The expression to convert to a column name</param>
        /// <param name="tableName">The table's name</param>
        /// <returns>A column name such as "TableName.ColumnName"</returns>
        public static string ToColumnName<T, U>(this Expression<Func<T, U>> expression, string tableName) =>
            $"{tableName}.{expression.ExtractMemberName()}";

        /// <summary>
        /// Converts an expression to a select clause for a table with the name of <typeparamref name="E"/> + 's'
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<E, V>(this Expression<Func<E, V>> expression, string leftTableName = null)
        {
            var rightTableName = typeof(E).ToTableName();

            return expression.ToSelectClause(leftTableName ?? rightTableName, rightTableName);
        }

        /// <summary>
        /// Converts an expression to a select clause for a table with the name <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <param name="tableName">The table's name</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<E, V>(this Expression<Func<E, V>> expression, string leftTableName, string rightTableName = null) =>
            $"SELECT {rightTableName ?? leftTableName}.{expression.ExtractMemberName()} FROM {leftTableName}";

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
            $"UPDATE {tableName} SET {expression.ToColumnName()}=@{valueName}";

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
        /// <returns>A WHERE clause such as "WHERE SomeThings.X=@Y"</returns>
        public static string ToWhereClause<E, K>(this Expression<Func<E, K>> expression, string valueName)
        {
            return expression.ToWhereClause(typeof(E).ToTableName(), valueName);
        }

        /// <summary>
        /// Converts an expression to a WHERE clause
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="K">The key's type</typeparam>
        /// <param name="expression">The expression to convert to a WHERE clause</param>
        /// <param name="tableName">The table's name</param>
        /// <param name="valueName">The name of the template value for the WHERE clause</param>
        /// <returns>A WHERE clause such as "WHERE SomeThings.X=@Y"</returns>
        public static string ToWhereClause<E, K>(this Expression<Func<E, K>> expression, string tableName, string valueName)
        {
            return SqlHelper.CreateWhereClause($"{expression.ToColumnName(tableName)}", valueName);
        }

        /// <summary>
        /// Converts an expression to a key value pair
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The value's type</typeparam>
        /// <param name="expression">A member access expression to select the column</param>
        /// <param name="valueName">The template value's name</param>
        /// <returns>A key value pair such as "X=@Y"</returns>
        public static string ToKeyValuePair<E, V>(this Expression<Func<E, V>> expression, string valueName) =>
            expression.ToKeyValuePair(typeof(E).ToTableName(), valueName);

        /// <summary>
        /// Converts an expression to a key value pair
        /// </summary>
        /// <typeparam name="E">The entity's type</typeparam>
        /// <typeparam name="V">The value's type</typeparam>
        /// <param name="expression">A member access expression to select the column</param>
        /// <param name="tableName">The table's name</param>
        /// <param name="valueName">The template value's name</param>
        /// <returns>A key value pair such as "X=@Y"</returns>
        public static string ToKeyValuePair<E, V>(this Expression<Func<E, V>> expression, string tableName, string valueName) =>
            $"{expression.ToColumnName(tableName)}=@{valueName}";

        /// <summary>
        /// Converts an expression to a JOIN clause
        /// </summary>
        /// <typeparam name="EL">The left table's entity type</typeparam>
        /// <typeparam name="ER">The right table's entity type</typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <param name="leftTableName">The table's name, auto-generated if null</param>
        /// <param name="rightTableName">The table's name, auto-generated if null</param>
        /// <returns>A JOIN clause</returns>
        public static string ToJoinClause<EL, ER, K>(
            this Expression<Func<EL, K>> leftExpression,
            Expression<Func<ER, K>> rightExpression,
            string leftTableName = null,
            string rightTableName = null) =>
            $"JOIN {rightTableName ?? typeof(ER).ToTableName()} " +
            $"ON {leftTableName ?? typeof(EL).ToTableName()}.{leftExpression.ExtractMemberName()} " +
            $"= {rightTableName ?? typeof(ER).ToTableName()}.{rightExpression.ExtractMemberName()}";
    }
}
