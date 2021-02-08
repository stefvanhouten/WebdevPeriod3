using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebdevPeriod3.Models;

namespace WebdevPeriod3.ViewModels
{
    public class AdminViewModel
    {
        public List<User> Users { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
