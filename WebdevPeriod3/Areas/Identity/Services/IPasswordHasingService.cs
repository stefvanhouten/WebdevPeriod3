namespace WebdevPeriod3.Areas.Identity.Services
{
    /// <summary>
    /// Defines common methods for password hashing services
    /// </summary>
    public interface IPasswordHasingService
    {
        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>The password's hash</returns>
        string Hash(string password);

        /// <summary>
        /// Compares a password to a hash
        /// </summary>
        /// <param name="password">The password to be compared to the hash</param>
        /// <param name="hash">The hash to be compared to the password</param>
        /// <returns><see langword="true"/> if the password and the hash match, otherwhise <see langword="false"/></returns>
        string Verify(string password, string hash);
    }
}
