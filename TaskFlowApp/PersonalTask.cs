namespace TaskFlowApp;

public class PersonalTask : TaskItem
{
    public string Category { get; set; }
    public PersonalTask(Guid taskId, string title, string? description, DateTime dueDate, string category, string status = "new", string priority = "")
                    : base(taskId, title, description, dueDate, status, priority)
    {
        this.Category = category;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($@"The details for Personal TaskItem with Id = {this.TaskId} are :
                            Title -> {this.Title} 
                            Description -> {this.Description??"No description"}
                            Status -> {this.Status}
                            Due date -> {this.DueDate?.ToShortDateString() ?? "No due date"}
                            Priority -> {this.Priority}
                            Category -> {this.Category}
                            ");
    }

}
