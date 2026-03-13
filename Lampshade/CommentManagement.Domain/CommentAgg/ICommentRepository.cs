using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;

namespace CommentManagement.Domain.CommentAgg;

public interface ICommentRepository : IRepository<long, Comment>
{
    List<CommentViewModel> Search(CommentSearchModel searchModel, bool showDeleted);
}