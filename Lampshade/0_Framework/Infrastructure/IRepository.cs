using _0_Framework.Domain;
using System.Linq.Expressions;
using _0_Framework.Application;

namespace _0_Framework.Infrastructure;

public interface IRepository<in TKey, T> where T : EntityBase<TKey>
{
    OperationResult Create(T entity);

    OperationResult Edit(T entity);

    T Get(TKey id);
    List<T> GetAll();
    bool Exists(Expression<Func<T, bool>> expression);
}