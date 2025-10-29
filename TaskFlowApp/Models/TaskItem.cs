using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public abstract class TaskItem : IEquatable<TaskItem>, IComparable<TaskItem>
{
    private string _title = string.Empty;
    private string? _description;
    
    private static IIdGenerator<Guid> _guidGenerator = new GuidGenerator();

    //do i need to provide validation for this field since the DueDate class already does at creation?
    //private DueDate _dueDate;
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 600;
    public Guid TaskId { get; }
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(value));
            }

            if (value.Length > MaxTitleLength)
            {
                throw new ArgumentException($"Title exceeds {MaxTitleLength} characters.");
            }

            _title = value;
        }
    }
    public string? Description
    {
        get => _description;
        set
        {
            if (value is not null && value.Length > MaxDescriptionLength)
            {
                throw new ArgumentException($"Description exceeds {MaxTitleLength} characters.");
            }

            _description = value;
        }
    }
    public Status Status { get; set; }
    public DueDate? DueDate { get; set; }
    public Priority Priority { get; set; }

    protected TaskItem(string title, string? description = null, DueDate? dueDate = null, Status status = Status.New, Priority priority = Enums.Priority.Low)
    {
        this.TaskId = _guidGenerator.CreateId();
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

    public void ChangeStatus(Status newStatus)
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
        return obj is TaskItem && Equals((TaskItem)obj);
    }

    public override int GetHashCode()
    {
        return this.TaskId.GetHashCode();
    }

    public int CompareTo(TaskItem? other)
    {
        if (other == null) return 1;

        //compare by dueDate
        if (this.DueDate is not null && other.DueDate is not null)
        {
            int dueDateComparison = DateTime.Compare(this.DueDate.Deadline, other.DueDate.Deadline);
            if (dueDateComparison != 0)
            {
                return dueDateComparison;
            }
        }
        //compare by priority
        //compareTo uses enum natural order since they have numbers assigned 
        return this.Priority.CompareTo(other.Priority);
    }

    public override string ToString()
    {
        return $@"The details for Work TaskItem with Id = {this.TaskId} are :
                            Title -> {this.Title} 
                            Description -> {this.Description ?? "No description"}
                            Status -> {this.Status}
                            Due date -> {this.DueDate?.ToString() ?? "No due date"}
                            Priority -> {this.Priority}";
    }

}
