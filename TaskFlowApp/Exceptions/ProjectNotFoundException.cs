namespace TaskFlowApp.Exceptions;

public class ProjectNotFoundException :  Exception
{
    public Guid ProjectCode { get; }
    public ProjectNotFoundException()
    {}

    public ProjectNotFoundException(Guid projectCode) : base($"Project with Id {projectCode} was not found.")
    {
        ProjectCode = projectCode;
    }

    public ProjectNotFoundException(string message) : base(message)
    {}
    
    public ProjectNotFoundException(string message, Exception inner) : base(message, inner)
    {}
}