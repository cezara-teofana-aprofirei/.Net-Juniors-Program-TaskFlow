namespace TaskFlowApp.Exceptions;

public class InvalidTaskOperationException : Exception
{
    public InvalidTaskOperationException()
    {}

    public InvalidTaskOperationException(string message) : base(message)
    {}
    
    public InvalidTaskOperationException(string message, Exception inner) : base(message, inner)
    {}
}
