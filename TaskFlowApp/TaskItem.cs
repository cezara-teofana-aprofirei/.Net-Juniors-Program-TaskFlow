using System;

namespace TaskFlowApp;

public abstract class TaskItem
{
    public Guid TaskId { get; private set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; }

    public TaskItem(Guid taskId, string title, string? description = null, DateTime? dueDate = null, string status = "New", string priority = "")
    {
        this.TaskId = taskId;
        this.Title = title;
        this.Description = description;
        this.Status = status;
        this.DueDate = dueDate;
        this.Priority = priority;
    }

    public void changeStatus(string newStatus)
    {
        this.Status = newStatus;
    }

    public abstract void DisplayDetails();
}
