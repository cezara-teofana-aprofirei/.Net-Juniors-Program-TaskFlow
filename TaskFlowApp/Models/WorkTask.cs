using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class WorkTask : TaskItem, ITask
{
    public string Department { get; set; }
    public WorkTask(string title, string? description, DueDate? dueDate, string department, Status status = Status.New, Priority priority = Priority.Low)
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
        Console.WriteLine(this.ToString());
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}\n Department -> {this.Department}";
    }
}
