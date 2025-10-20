using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public abstract class TaskItem : IEquatable<TaskItem>, IComparable<TaskItem>
{
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 600;
    public Guid TaskId { get; private set; }
    public string Title{ get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
    public DueDate? DueDate { get; set; }
    public Priority Priority { get; set; }

    protected TaskItem(string title, string? description = null, DueDate? dueDate = null, Status status = Status.New, Priority priority = Enums.Priority.Low)
    {
        this.TaskId = Guid.NewGuid();
        this.Title = title;
        this.Description = description;
        this.Status = status;
        this.DueDate = dueDate;
        this.Priority = priority;
    }

    //copy constructor
    protected TaskItem(TaskItem existingTaskItem)
    {
        this.TaskId = existingTaskItem.TaskId;
        this.Title = existingTaskItem.Title;
        this.Description = existingTaskItem.Description;
        this.Status = existingTaskItem.Status;
        this.DueDate = existingTaskItem.DueDate;
        this.Priority = existingTaskItem.Priority;
    }

    public void changeStatus(Status newStatus)
    {
        if (this.Status == Status.Completed && newStatus != Status.Completed)
        {
            throw new InvalidTaskOperationException("Completed tasks cannot be reopened!");
        }
        
        this.Status = newStatus;
    }

    public abstract void DisplayDetails();

    public bool Equals(TaskItem? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.TaskId == other.TaskId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is TaskItem && Equals((TaskItem)obj))
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return this.TaskId.GetHashCode();
    }

    public int CompareTo(TaskItem? other)
    {
        if (other == null) return 1;

        //compare by dueDate
        int dueDateComparison = DateTime.Compare(this.DueDate.Deadline, other.DueDate.Deadline);
        if (dueDateComparison != 0)
        {
            return dueDateComparison;
        }

        //compare by priority
        //compareTo uses enum natural order since they have numbers assigned 
        return this.Priority.CompareTo(other.Priority);
    }

}
