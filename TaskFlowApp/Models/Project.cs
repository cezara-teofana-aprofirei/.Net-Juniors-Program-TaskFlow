using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.Interfaces;

namespace TaskFlowApp.Models;

public class Project
{
    public Guid ProjectCode { get; private set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
    private static IIdGenerator<Guid> _guidGenerator = new GuidGenerator();

    public Project(string name, string? description=null)
    {
        this.ProjectCode = _guidGenerator.CreateId();
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
            taskItem.DisplayDetails();
        }
        Console.WriteLine();
    }

    public void CompleteTask(TaskItem task)
    {
        bool foundTask = false;
        foreach (TaskItem item in Tasks)
        {
            if (item.TaskId == task.TaskId)
            {
                if (item.Status == Status.Completed)
                {
                    Console.WriteLine("Task already completed!");
                    return;
                }
                foundTask = true;
            }
        }

        if (foundTask == false)
        {
            throw new TaskNotFoundException(task.TaskId);
        }
        
        task.ChangeStatus(Status.Completed);
        Console.WriteLine($"Task {task.Title} completed successfully!");
    }

    public void CompleteAllForProject()
    {
        foreach (TaskItem task in Tasks)
        {
            task.ChangeStatus(Status.Completed);
        }
        Console.WriteLine($"Completed all tasks for project with ID :  {this.ProjectCode}");
    }

    public void ListOverdue()
    {
        foreach (TaskItem task in Tasks)
        {
            if (task.DueDate is not null && DateTime.Compare(task.DueDate.Deadline, DateTime.Today) < 0 && task.Status != Status.Completed)
            {
                //overdue
                Console.WriteLine($"Task {task.Title} with ID : {task.TaskId}, due date : {task.DueDate.Deadline}, status : {task.Status} is overdue");
            }
        }
    }
    
}

