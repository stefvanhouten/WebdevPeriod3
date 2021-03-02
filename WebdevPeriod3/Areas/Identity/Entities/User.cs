using Microsoft.AspNetCore.Identity;

namespace WebdevPeriod3.Areas.Identity.Entities
{
    /// <summary>
    /// Represents a user within our application
    /// </summary>
    public class User : IdentityUser<string> {
        public User() : base() { }

        public User(string userName) : base(userName) { }
    }
}
