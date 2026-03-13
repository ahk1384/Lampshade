using _0_Framework.Application;

namespace CommentManagement.Application.Contracts.Comment;

public interface ICommentApplication
{
    OperationResult Add(AddComment command);
    OperationResult Confirm(long id);
    OperationResult Cancel(long id);

    OperationResult Restore(long id);
    List<CommentViewModel> Search(CommentSearchModel searchModel, bool showDeleted);
}