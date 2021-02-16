using System;

namespace WebdevPeriod3.Utilities
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Convert this type to a table name
        /// </summary>
        /// <param name="type">The type to be converted to a table name</param>
        /// <returns><paramref name="type"/>'s name, converted to lower case and suffixed with an 's'</returns>
        public static string ToTableName(this Type type)
            => type.Name.ToLower() + 's';

        /// <summary>
        /// Converts this type to a SELECT query
        /// </summary>
        /// <param name="type">The type to be converted to a SELECT query</param>
        /// <returns>
        /// A SELECT query that selects all columns
        /// from a table with the lower-case name of <paramref name="type"/> + 's'
        /// </returns>
        public static string ToSelectQuery(this Type type)
            => SqlHelper.CreateSelectQuery(type.ToTableName());
    }
}
