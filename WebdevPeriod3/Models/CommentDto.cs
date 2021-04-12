using System.ComponentModel.DataAnnotations;

namespace WebdevPeriod3.Models
{
    public class CommentDto
    {
        [Required]
        [Display(Name = "Inhoud")]
        public string Content { get; set; }
        public string ParentId { get; set; } = null;

        public CommentDto() { }

        public CommentDto(ViewModels.CommentViewModel commentViewModel) : this()
        {
            ParentId = commentViewModel.Id;
        }
    }
}
