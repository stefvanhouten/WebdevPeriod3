using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Areas.Identity.Entities
{
    /// <summary>
    /// Represents a role within our application
    /// </summary>
    public class Role : IdentityRole<string> {
        public Role(): base() { }

        public Role(string roleName) : base(roleName) { }
    }
}
