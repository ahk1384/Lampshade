using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DiscountManagemenet.Infrastructure.EFCore;

public class UnitOfWorkDiscount : IUnitOfWork
{
    private readonly DiscountContext _context;

    public UnitOfWorkDiscount(DiscountContext context)
    {
        _context = context;
    }

    public void BeginTran()
    {
        _context.Database.BeginTransaction();
    }

    public void CommitTran()
    {
        _context.Database.CommitTransaction();
        _context.SaveChanges();
    }

    public void Rollback()
    {
        _context.Database.RollbackTransaction();
    }
}