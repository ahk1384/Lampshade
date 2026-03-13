using _0_Framework.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Application;

public class CommentApplication : ICommentApplication
{
    private readonly ICommentRepository _commentRepository;

    public CommentApplication(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public OperationResult Add(AddComment command)
    {
        var operationResult = new OperationResult();
        _commentRepository.BeginTran();
        try
        {
            var comment = new Comment(command.Name, command.Email, command.Website,command.Message, command.OwnerRecordId, command.Type,
                command.ParentId);
            _commentRepository.Create(comment);
        }
        catch (Exception e)
        {
            _commentRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _commentRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Confirm(long id)
    {
        var operationResult = new OperationResult();
        _commentRepository.BeginTran();
        try
        {
            var discount = _commentRepository.Get(id);
            discount.Confirm();
        }
        catch (Exception e)
        {
            _commentRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _commentRepository.CommitTran();
        return operationResult.Success();
    }

    public OperationResult Cancel(long id)
    {
        var operationResult = new OperationResult();
        _commentRepository.BeginTran();
        try
        {
            var discount = _commentRepository.Get(id);
            discount.Remove();
        }
        catch (Exception e)
        {
            _commentRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _commentRepository.CommitTran();
        return operationResult.Success();

    }

    public OperationResult Restore(long id)
    {
        var operationResult = new OperationResult();
        _commentRepository.BeginTran();
        try
        {
            var discount = _commentRepository.Get(id);
            discount.Restore();
        }
        catch (Exception e)
        {
            _commentRepository.Rollback();
            return operationResult.Fail(e.Message);
        }

        _commentRepository.CommitTran();
        return operationResult.Success();
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel,bool showDeleted)
    {
        return _commentRepository.Search(searchModel, showDeleted);
    }
}