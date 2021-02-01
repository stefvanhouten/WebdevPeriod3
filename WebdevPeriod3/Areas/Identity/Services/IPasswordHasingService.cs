namespace WebdevPeriod3.Areas.Identity.Services
{
    /// <summary>
    /// Defines common methods for password hashing services
    /// </summary>
    public interface IPasswordHasingService
    {
        /// <summary>
        /// Defines the available costs for hashing strings
        /// </summary>
        public enum PasswordHashingCost
        {
            /// <summary>
            /// This cost should be used for hashing tokens
            /// </summary>
            LOW,
            /// <summary>
            /// This cost should be used for hashing passwords
            /// </summary>
            HIGH
        }

        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <param name="cost">The cost to be used for hashing the password</param>
        /// <returns>The password's hash</returns>
        string Hash(string password, PasswordHashingCost cost);

        /// <summary>
        /// Compares a password to a hash
        /// </summary>
        /// <param name="password">The password to be compared to the hash</param>
        /// <param name="hash">The hash to be compared to the password</param>
        /// <returns><see langword="true"/> if the password and the hash match, otherwhise <see langword="false"/></returns>
        string Verify(string password, string hash);
    }
}
