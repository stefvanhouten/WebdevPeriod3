using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Areas.Identity.Entities;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.ViewModels
{
    public class ProfileViewModel
    {
        public User UserInformation { get; set; }
        public IEnumerable<Product> OwnProducts { get; set; }
    }
}
