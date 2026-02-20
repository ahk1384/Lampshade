namespace _0_Framework.Domain;

public class EntityBase<T>
{
    public T Id { get; private set; }
    public DateTime CreationDate { get; private set; }

    public bool IsDeleted { get; private set; }
    public EntityBase()
    {
        CreationDate = DateTime.Now;
        IsDeleted = false;
    }

    public void Remove()
    {
        IsDeleted = true;
    }

    public void Active()
    {
        IsDeleted = false;
    }
}