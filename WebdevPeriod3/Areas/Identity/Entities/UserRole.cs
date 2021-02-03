using Microsoft.AspNetCore.Identity;

namespace WebdevPeriod3.Areas.Identity.Entities
{
    /// <summary>
    /// Represents a link between a user and a role in our application
    /// </summary>
    public class UserRole : IdentityUserRole<string> { }
}
