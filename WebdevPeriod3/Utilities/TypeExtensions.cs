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
    }
}
