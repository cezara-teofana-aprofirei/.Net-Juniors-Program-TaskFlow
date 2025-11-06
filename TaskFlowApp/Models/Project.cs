using System.Text.Json.Serialization;
using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.Interfaces;

namespace TaskFlowApp.Models;

public class Project : IEquatable<Project>, IIdentifiable<Guid>
{
    public Guid ProjectCode { get; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<TaskItem> Tasks { get; init; } = new List<TaskItem>();
    
    private static IIdGenerator<Guid> _guidGenerator = new GuidGenerator();

    public static void ConfigureGenerator(IIdGenerator<Guid> generator)
    {
        _guidGenerator = generator;
    }

    public Project(string name, string? description=null)
    {
        this.ProjectCode = _guidGenerator.CreateId();
        this.Name = name;
        this.Description = description;
    }
    
    [JsonConstructor]
    public Project(Guid projectCode, string name, string? description, List<TaskItem>? tasks)
    {
        ProjectCode = projectCode;
        Name = name;
        Description = description;
        if (tasks is not null) Tasks.AddRange(tasks);
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
        foreach (var taskItem in Tasks)
        {
            taskItem.DisplayDetails();
        }
        Console.WriteLine();
    }

    public void CompleteTask(TaskItem task)
    {
        bool foundTask = false;
        foreach (var item in Tasks)
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
                Console.WriteLine($"Task {task.Title} with ID : {task.TaskId}, due date : {task.DueDate.Deadline}, status : {task.Status} is overdue");
            }
        }
    }
    
    public bool Equals(Project? other)
    {
        if (other is null)
        {
            return false;
        }
        return this.ProjectCode == other.ProjectCode;
    }

    public Guid GetId()
    {
        return ProjectCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is Project && Equals((Project)obj);
    }
    
    public override int GetHashCode()
    {
        return this.ProjectCode.GetHashCode();
    }

    public override string ToString()
    {
        var taskTitles = string.Join(" ,", Tasks.Select(t => t.Title));
        return
            $"Project Code : {this.ProjectCode} \n Title : {this.Name} \n Description : {this.Description} \n Tasks : {taskTitles}";
    }
}

