namespace _0_Framework.Infrastructure;

public interface IUnitOfWork
{
    void BeginTran();
    void CommitTran();
    void Rollback();
}