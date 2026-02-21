using System.Linq.Expressions;
using _0_Framework.Domain;

namespace _0_Framework.Infrastructure;

public interface IRepository<in TKey, T> where T : EntityBase<TKey>
{
    void Create(T entity);

    void Edit(T entity);

    void Remove(TKey id);

    void Restore(TKey id);
    T Get(TKey id);
    List<T> GetAll();
    bool Exists(Expression<Func<T, bool>> expression);
}