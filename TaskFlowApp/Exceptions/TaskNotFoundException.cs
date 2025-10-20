namespace TaskFlowApp.Exceptions;

public class TaskNotFoundException : Exception
{
    public Guid TaskId { get; }
    public TaskNotFoundException()
    {}

    public TaskNotFoundException(Guid taskId) : base($"Task with Id {taskId} was not found.")
    {}

    public TaskNotFoundException(string message) : base(message)
    {}
    
    public TaskNotFoundException(string message, Exception inner) : base(message, inner)
    {}
}
