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
        /// Converts an expression to a select clause for a table with the name of <typeparamref name="T"/> + 's'
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <typeparam name="U">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<T, U>(this Expression<Func<T, U>> expression) =>
            expression.ToSelectClause(typeof(U).Name.ToLower());

        /// <summary>
        /// Converts an expression to a select clause for a table with the name <paramref name="tableName"/>
        /// </summary>
        /// <typeparam name="T">The entity's type</typeparam>
        /// <typeparam name="U">The property's type</typeparam>
        /// <param name="expression">The expression to convert to a select clause</param>
        /// <returns>A select clause</returns>
        public static string ToSelectClause<T, U>(this Expression<Func<T, U>> expression, string tableName) =>
            $"SELECT {expression.ExtractMemberName()} FROM {tableName}";
    }
}
