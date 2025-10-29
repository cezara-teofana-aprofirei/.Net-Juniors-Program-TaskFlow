namespace TaskFlowApp.Interfaces;

public interface IIdGenerator<out T>
{
    T CreateId();
}

public class GuidGenerator : IIdGenerator<Guid>
{
    public Guid CreateId()
    {
        return  Guid.NewGuid();
    }
}