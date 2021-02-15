using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Areas.Identity.Entities
{
    /// <summary>
    /// Represents a user within our application
    /// </summary>
    public class User : IdentityUser<string> {
        public User() : base() { }

        public User(string userName) : base(userName) { }

        public string Role { get; internal set; }
    }
}
