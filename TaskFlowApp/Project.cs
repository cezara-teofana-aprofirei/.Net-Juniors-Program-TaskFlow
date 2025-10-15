namespace TaskFlowApp;

public class Project
{
    public string ProjectCode { get; private set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    public Project(string projectCode, string name, string description="")
    {
        this.ProjectCode = projectCode;
        this.Name = name;
        this.Description = description;
    }

    public void AddTask(TaskItem newTask)
    {
        Tasks.Add(newTask);
        Console.WriteLine($"Task {newTask.Title} added successfully!");
        Console.WriteLine();
    }

    public void ListTasks()
    {
        Console.WriteLine($"These are the tasks for project {this.Name}: ");
        foreach (TaskItem taskItem in Tasks)
        {
            Console.WriteLine(taskItem.Title); //TODO: replace title with display details after implementing
        }
        Console.WriteLine();
    }

    public void CompleteTask(TaskItem task)
    {
        //look for the task in the list and call the function if i can find it
        if (Tasks.Find(t => t.TaskId == task.TaskId) != null)
        {
            task.changeStatus("Complete");
            Console.WriteLine($"Task {task.Title} completed successfully!");
        }
        else
        {
            Console.WriteLine($"Couldn't find task in tasks list for project {this.Name}");
        }
    }

}
