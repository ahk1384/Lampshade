namespace _0_Framework.Domain;

public class EntityBase<T>
{
    public EntityBase()
    {
        CreationDate = DateTime.Now;
        IsDeleted = false;
    }

    public T Id { get; private set; }
    public DateTime CreationDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public void Remove()
    {
        IsDeleted = true;
    }

    public void Restore()
    {
        IsDeleted = false;
    }
}