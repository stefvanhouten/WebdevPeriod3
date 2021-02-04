using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebdevPeriod3.Entities
{
    /// <summary>
    /// Represents a comment in our application
    /// </summary>
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ProductId { get; set; }
        public string ParentId { get; set; }
        public string PosterId { get; set; }
        public bool Flagged { get; set; }
    }
}
