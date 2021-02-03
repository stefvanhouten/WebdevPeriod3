using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebdevPeriod3.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToKeyGroup<T>(this T values, Expression<Func<T, object>> keyExpression = null) =>
            values.ToKeyGroup(keyExpression?.ExtractMemberName());

        private static string ToKeyGroup<T>(this T _, string keyPropertyName) =>
            string.Join(
                ',',
                typeof(T).GetProperties()
                .Where(property => keyPropertyName == null || property.Name != keyPropertyName)
                .Select(property => $"{property.Name}=@{property.Name}")
            );

        public static string ToUpdateQuery<T>(this T values, Expression<Func<T, object>> keyExpression) =>
            values.ToUpdateQuery(typeof(T).Name.ToLower() + 's', keyExpression);

        public static string ToUpdateQuery<T>(this T values, string tableName, Expression<Func<T, object>> keyExpression)
        {
            var keyPropertyName = keyExpression.ExtractMemberName();

            return $"UPDATE {tableName} SET {values.ToKeyGroup(keyPropertyName)} WHERE {keyPropertyName}=@{keyPropertyName};";
        }

        public static string ToInsertQuery<T>(this T values) =>
            values.ToInsertQuery(typeof(T).Name.ToLower() + 's');

        public static string ToInsertQuery<T>(this T values, string tableName) =>
            $"INSERT INTO {tableName} SET {values.ToKeyGroup()};";
    }
}
