namespace TaskFlowApp.Exceptions;

public class InvalidProjectOperationException : Exception
{
    public InvalidProjectOperationException()
    {}

    public InvalidProjectOperationException(string message) : base(message)
    {}
    
    public InvalidProjectOperationException(string message, Exception inner) : base(message, inner)
    {}
}