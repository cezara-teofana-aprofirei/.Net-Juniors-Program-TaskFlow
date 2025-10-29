
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
        Console.WriteLine(this.ToString());
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}\n Category -> {this.Category}";
    }

}
