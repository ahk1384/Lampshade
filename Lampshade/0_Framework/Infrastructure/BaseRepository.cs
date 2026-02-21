using System.Linq.Expressions;
using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.Infrastructure;

public class BaseRepository<TKey, T> : IRepository<TKey, T> where T : EntityBase<TKey>
{
    private readonly DbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public void Create(T entity)
    {
        _context.Add<T>(entity);
    }

    public void Edit(T entity)
    {
        _context.Update<T>(entity);
    }

    public void Remove(TKey id)
    {
        var context = Get(id);
        context.Remove();
    }

    public void Restore(TKey id)
    {
        var context = Get(id);
        context.Restore();
        _unitOfWork.CommitTran();
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
}