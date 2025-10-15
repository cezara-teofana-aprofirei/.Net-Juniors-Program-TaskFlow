namespace TaskFlowApp;

public class WorkTask : TaskItem
{
    public string Department { get; set; }
    public WorkTask(Guid taskId, string title, string? description, DateTime dueDate, string department, string status = "new", string priority = "")
                    : base(taskId, title, description, dueDate, status, priority)
    {
        this.Department = department;
    }
    
    public override void DisplayDetails()
    {
        Console.WriteLine($@"The details for Work TaskItem with Id = {this.TaskId} are :
                            Title -> {this.Title} 
                            Description -> {this.Description??="No description"}
                            Status -> {this.Status}
                            Due date -> {this.DueDate?.ToShortDateString() ?? "No due date"}
                            Priority -> {this.Priority}
                            Department -> {this.Department}
                            ");
    }
}
