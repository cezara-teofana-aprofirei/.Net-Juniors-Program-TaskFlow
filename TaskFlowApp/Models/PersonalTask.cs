
using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class PersonalTask : TaskItem, ITask
{
    public string Category { get; set; }
    public PersonalTask(string title, string? description, DueDate? dueDate, string category, Status status = Status.New, Priority priority = Priority.Low)
                    : base(title, description, dueDate, status, priority)
    {
        this.Category = category;
    }

    //copy ctor
    public PersonalTask(PersonalTask existingTask) : base(existingTask)
    {
        this.Category = existingTask.Category;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($@"The details for Personal TaskItem with Id = {this.TaskId} are :
                            Title -> {this.Title} 
                            Description -> {this.Description??"No description"}
                            Status -> {this.Status}
                            Due date -> {this.DueDate?.ToString() ?? "No due date"}
                            Priority -> {this.Priority}
                            Category -> {this.Category}
                            ");
    }

}
