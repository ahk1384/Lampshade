using _0_Framework.Application;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain;
using CommentManagement.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;

namespace CommentManagement.Infrastructure.EFCore.Repositories;

public class CommentRepository : BaseRepository<long,Comment>,ICommentRepository
{
    private readonly CommentContext _context;
    public CommentRepository(CommentContext context) : base(context)
    {
        _context = context;
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel, bool showDeleted)
    {
        var query = _context.Comments
            .Select(x => new CommentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Website = x.Website,
                Message = x.Message,
                OwnerRecordId = x.OwnerRecordId,
                Type = x.Type,
                IsCanceled = x.IsDeleted,
                IsConfirmed = x.IsConfirmed,
                CommentDate = x.CreationDate.ToFarsi()
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name))
            query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Email))
            query = query.Where(x => x.Email.Contains(searchModel.Email));

        return query.OrderByDescending(x => x.Id).ToList();
    }

}