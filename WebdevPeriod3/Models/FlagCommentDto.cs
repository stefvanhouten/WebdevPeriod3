using System.ComponentModel.DataAnnotations;
using WebdevPeriod3.ViewModels;

namespace WebdevPeriod3.Models
{
    public class FlagCommentDto
    {
        [Required]
        public string Id { get; set; }

        public FlagCommentDto() { }

        public FlagCommentDto(CommentViewModel commentViewModel) : this()
        {
            Id = commentViewModel.Id;
        }
    }
}
