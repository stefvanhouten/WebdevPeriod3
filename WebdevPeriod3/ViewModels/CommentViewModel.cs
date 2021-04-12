using System.Collections.Generic;

namespace WebdevPeriod3.ViewModels
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string PosterName { get; set; }
        public string Content { get; set; }
        public IEnumerable<CommentViewModel> Replies { get; set; }

        public CommentViewModel(string id, string posterName, string content, IEnumerable<CommentViewModel> replies)
        {
            Id = id;
            PosterName = posterName;
            Content = content;
            Replies = replies;
        }
    }
}
