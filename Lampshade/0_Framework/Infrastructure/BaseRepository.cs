using System.Linq.Expressions;
using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.Infrastructure;

public class BaseRepository<TKey, T> : IRepository<TKey, T> where T : EntityBase<TKey>
{
    private readonly DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public void Create(T entity)
    {
        _context.Add<T>(entity);
    }

    public T Get(TKey id)
    {
        return _context.Find<T>(id);
    }

    public List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Any(expression);
    }

    public void CommitTran()
    {
        _context.Database.CommitTransaction();
        _context.SaveChanges();
    }

    public void BeginTran()
    {
        _context.Database.BeginTransaction();
    }

    public void Rollback()
    {
        _context.Database.RollbackTransaction();
    }
}
