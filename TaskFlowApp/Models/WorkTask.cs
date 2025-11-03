using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class WorkTask : TaskItem
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
        return $"{base.ToString()}\n Department -> {this.Department}\n Type -> Work task";
    }

    public override void CopySpecificType(TaskItem other)
    {
        if (other is WorkTask task)
        {
            this.Department = task.Department;
        }
    }
}
