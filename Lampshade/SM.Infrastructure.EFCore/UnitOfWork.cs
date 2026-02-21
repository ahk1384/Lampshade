using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SM.Infrastructure.EFCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShopContext _context;

    public UnitOfWork(ShopContext context)
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