using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;

namespace TaskFlowApp.Models;

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
            taskItem.DisplayDetails();
        }
        Console.WriteLine();
    }

    public void CompleteTask(TaskItem task)
    {
        //look for the task in the list and call the function if i can find it
        
        bool foundTask = false;
        foreach (TaskItem item in Tasks)
        {
            if (item.TaskId == task.TaskId)
            {
                foundTask = true;
            }
        }

        if (foundTask == false)
        {
            throw new TaskNotFoundException(task.TaskId);
        }
        
        task.changeStatus(Status.Completed);
        Console.WriteLine($"Task {task.Title} completed successfully!");
    }

    public void CompleteAllForProject()
    {
        foreach (TaskItem task in Tasks)
        {
            task.changeStatus(Status.Completed);
        }
        Console.WriteLine($"Completed all tasks for project with ID :  {this.ProjectCode}");
    }
    
    public void ListOverdue()
    {
        foreach (TaskItem task in Tasks)
        {
            if(DateTime.Compare(task.DueDate.Deadline, DateTime.Today)<0 && task.Status!=Status.Completed)
            {
                //overdue
                Console.WriteLine($"Task {task.Title} with ID : {task.TaskId}, due date : {task.DueDate.Deadline}, status : {task.Status} is overdue");
            }
        }
    }

}

