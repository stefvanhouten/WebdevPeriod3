using System.Collections.Generic;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Services
{
    public class DapperCommentStore
    {
        private readonly CommentRepository _commentRepository;
        private readonly DapperTransactionService _dapperTransactionService;

        public DapperCommentStore(CommentRepository commentRepository, DapperTransactionService dapperTransactionService)
        {
            _commentRepository = commentRepository;
            _dapperTransactionService = dapperTransactionService;
        }

        public Task AddComment(Comment comment)
        {
            _commentRepository.AddComment(comment);
            return _dapperTransactionService.RunOperations();
        }

        public Task AddComment(string content, string productId, string parentId, string posterId) => AddComment(new Comment()
        {
            Content = content,
            ProductId = productId,
            ParentId = parentId,
            PosterId = posterId
        });

        public Task<IEnumerable<Models.HydratedComment>> GetComments(string productId) => _commentRepository.GetComments(productId);

        public Task FlagComment(string commentId)
        {
            _commentRepository.FlagComment(commentId);
            return _dapperTransactionService.RunOperations();
        }
    }
}
