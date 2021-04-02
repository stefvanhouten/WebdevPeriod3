using System.Collections.Generic;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.ViewModels
{
    public class DashboardViewModel
    {
        public string SearchTerm { get; set; }
        public List<Product> ProductPosts { get; set; }
    }
}
