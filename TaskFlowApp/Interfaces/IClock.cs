namespace TaskFlowApp.Interfaces;

public interface IClock
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}