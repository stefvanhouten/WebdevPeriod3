using System;
using System.Security.Cryptography;

namespace WebdevPeriod3.Areas.Identity.Services
{
    [Obsolete("This implementation will be replaced with the built-in version.")]
    /// <summary>
    /// An Argon2-based implementation for password hashing
    /// </summary>
    abstract public class Argon2PasswordHashingService : IPasswordHasingService
    {
        /// <summary>
        /// A cryptographically secure random number generator
        /// </summary>
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Generates a salt of the length defined in 
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateSalt(IPasswordHasingService.PasswordHashingCost cost)
        {
            byte[] salt = new byte[
                cost switch
                {
                    IPasswordHasingService.PasswordHashingCost.LOW => 8,
                    IPasswordHasingService.PasswordHashingCost.HIGH => 16,
                    _ => throw new NotImplementedException(
                        $"There is no Argon2 salting implementation for the following cost: {cost}."
                        )
                }
                ];

            Rng.GetBytes(salt);

            return salt;
        }

        public string Hash(string password, IPasswordHasingService.PasswordHashingCost cost)
        {
            throw new NotImplementedException();
        }

        public string Verify(string password, string hash)
        {
            throw new NotImplementedException();
        }
    }
}
