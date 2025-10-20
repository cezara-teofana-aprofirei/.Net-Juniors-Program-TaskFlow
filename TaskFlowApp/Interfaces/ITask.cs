using TaskFlowApp.Enums;
using TaskFlowApp.Models;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Interfaces;

public interface ITask
{
    Guid TaskId { get; }
    string Title { get; set; }
    string? Description { get; set; }
    Status Status { get; set; }
    DueDate? DueDate { get; set; }
    Priority Priority { get; set; }

    void changeStatus(Status newStatus);
    void DisplayDetails();
}