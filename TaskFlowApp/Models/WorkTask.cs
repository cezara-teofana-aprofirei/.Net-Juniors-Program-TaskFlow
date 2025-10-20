using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class WorkTask : TaskItem, ITask
{
    public string Department { get; set; }
    public WorkTask(string title, string? description, DueDate dueDate, string department, Status status = Status.New, Priority priority = Priority.Low)
                    : base(title, description, dueDate, status, priority)
    {
        this.Department = department;
    }
    
    //copy ctor
    public WorkTask(WorkTask existingTask) : base(existingTask)
    {
        this.Department = existingTask.Department;
    }
    
    public override void DisplayDetails()
    {
        Console.WriteLine($@"The details for Work TaskItem with Id = {this.TaskId} are :
                            Title -> {this.Title} 
                            Description -> {this.Description??"No description"}
                            Status -> {this.Status}
                            Due date -> {this.DueDate?.ToString() ?? "No due date"}
                            Priority -> {this.Priority}
                            Department -> {this.Department}
                            ");
    }
}
