
using System.Text.Json.Serialization;
using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class PersonalTask : TaskItem
{
    public string? Category { get; set; }
    public PersonalTask(string title, string? description, DueDate? dueDate, string category, Status status = Status.New, Priority priority = Priority.Low)
                    : base(title, description, dueDate, status, priority)
    {
        this.Category = category;
    }
    
    [JsonConstructor]
    public PersonalTask(Guid taskId, string title, string? description, DueDate? dueDate,
        string category, Status status, Priority priority)
        : base(taskId, title, description, dueDate, status, priority)
    {
        Category = category;
    }

    //copy ctor
    public PersonalTask(PersonalTask existingTask) : base(existingTask)
    {
        this.Category = existingTask.Category;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine(this.ToString());
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}\n Category -> {this.Category}\n Type -> Personal task";
    }

    public override void CopySpecificType(TaskItem other)
    {
        if (other is PersonalTask task)
        {
            this.Category = task.Category;
        }
    }
}
