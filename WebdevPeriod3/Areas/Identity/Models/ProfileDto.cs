using System.Collections.Generic;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Areas.Identity.Models
{
    public class ProfileDto
    {
        public User UserInformation { get; set; }
        public IEnumerable<Product> OwnProducts { get; set; }
    }
}

