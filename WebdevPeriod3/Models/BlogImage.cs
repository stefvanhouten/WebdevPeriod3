using System.Collections.Generic;

namespace WebdevPeriod3.Models
{
    public class BlogImage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public List<string> WebshopUrl { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
