using System.Linq.Expressions;
using _0_Framework.Application;
using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _0_Framework.Infrastructure;

public class BaseRepository<TKey,T> : IRepository<TKey,T> where T : EntityBase<TKey>
{
    private readonly DbContext _context;

    public BaseRepository(DbContext context)
    {
        _context = context;
    }

    public OperationResult Create(T entity)
    {
        throw new NotImplementedException();
    }

    public OperationResult Edit(T entity)
    {
        throw new NotImplementedException();
    }

    public T Get(TKey id)
    {
        throw new NotImplementedException();
    }

    public List<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }
}