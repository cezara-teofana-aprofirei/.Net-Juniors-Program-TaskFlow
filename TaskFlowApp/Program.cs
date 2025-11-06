using TaskFlowApp.Common;
using TaskFlowApp.Enums;
using TaskFlowApp.Models;
using TaskFlowApp.ValueObjects;

#region AddingData
Project testProject = new Project(".NET practice project", "A project meant to develop .Net programming skills");
Project testProjectJava = new Project("Java practice project", "A project meant to develop Java programming skills");

DueDate deadlinePersonalTask = new DueDate(DateTime.Today.AddDays(-1), true);
DueDate deadlinePersonalTask2 = new DueDate(DateTime.Today.AddDays(2), true);
DueDate deadlinePersonalTask3 = new DueDate(DateTime.Today.AddDays(5), true);
DueDate deadlineWorkTask = new DueDate(DateTime.Today.AddDays(3));



PersonalTask personalTask = new("Walk dog", "", deadlinePersonalTask, priority: Priority.Medium, category: "Pets");       
//PersonalTask taskNotInProject = new("Project unrelated task", "", deadlinePersonalTask, "Sports");

WorkTask workTask = new("Refactor code", "Refactor code for the console app", deadlineWorkTask, "Development", priority: Priority.High, status: Status.Completed);

testProject.AddTask(personalTask);
testProject.AddTask(workTask);

//add some copies for the personalTask to demonstrate sorting
PersonalTask personalTask2 = new("Walk dog", "", deadlinePersonalTask2, priority: Priority.Medium, category: "Pets");
PersonalTask personalTask3 = new("Walk cat", "", deadlinePersonalTask3, priority: Priority.Medium, category: "Pets");
PersonalTask personalTask4 = new("Walk cat", "", deadlinePersonalTask3, priority: Priority.High, category: "Pets");

testProject.AddTask(personalTask2);
testProject.AddTask(personalTask3);
testProject.AddTask(personalTask4);

testProjectJava.AddTask(personalTask2);
testProjectJava.AddTask(personalTask3);

//add the tasks in the json
//IRepository<TaskItem, Guid> taskItemRepository = new TaskRepository();
// await taskItemRepository.AddAsync(personalTask);
// await taskItemRepository.AddAsync(personalTask2);
// await taskItemRepository.AddAsync(personalTask3);
// await taskItemRepository.AddAsync(personalTask4);
  
//await taskItemRepository.AddAsync(workTask);
#endregion

// #region Testing Phase1 functionalities
// //method which lists all tasks from a project
// Console.WriteLine("ListTasks function call:");
// testProject.ListTasks();
//
// //demonstrate complete task method
// Console.WriteLine("Complete task function call");
// testProject.CompleteTask(workTask);
//
// //method to display details, behaves differently based on the task type
// Console.WriteLine("DisplayDetails function call for tasks in Tasks list:");
// foreach (TaskItem task in testProject.Tasks)
// {
//     task.DisplayDetails();
// }
//
// //demonstrating deduplication for tasks 
// //the hashset uses the equals method from TaskItem to decide which tasks are unique
// var copyOfPersonalTask1 = new PersonalTask(personalTask);
// testProject.AddTask(copyOfPersonalTask1);
//
// Console.WriteLine("This is the list with duplicated tasks");
// foreach (TaskItem task in testProject.Tasks)
// {
//     Console.WriteLine($"Task with title : {task.Title} and id : {task.TaskId} ");
// }
// Console.WriteLine();
//
// Console.WriteLine("This is the list with de-duplicated tasks");
// var deduplicatedTasks = new HashSet<TaskItem>(testProject.Tasks);
//
// foreach (TaskItem task in deduplicatedTasks)
// {
//     Console.WriteLine($"Task with title : {task.Title} and id : {task.TaskId} ");
// }
// Console.WriteLine();
//
//
// //demonstrate sorting for TaskItem objects
// Console.WriteLine("This is the list with unsorted tasks");
//
// foreach (TaskItem task in testProject.Tasks)
// {
//     Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate?.Deadline}, priority : {task.Priority} ");
// }
// Console.WriteLine();
//
// Console.WriteLine("This is the list with sorted tasks");
// testProject.Tasks.Sort();
// foreach (TaskItem task in testProject.Tasks)
// {
//     Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate?.Deadline}, priority : {task.Priority} ");
// }
// Console.WriteLine();
//
// //showcase ListOverdue and CompleteAllForProject methods
// Console.WriteLine("Listing overdue projects");
// testProject.ListOverdue();
//
// //completing all projects
// Console.WriteLine("Complete all tasks for project method call:");
// testProject.CompleteAllForProject();
//
// //listing all overdue again, the completed projects won't appear anymore
// Console.WriteLine("Listing overdue projects after completing all");
// testProject.ListOverdue();
//
// //demonstrate custom exceptions
// Console.WriteLine("Demonstrating custom exceptions:");
// try
// {
//     testProject.CompleteTask(taskNotInProject);
// }
// catch (TaskNotFoundException ex)
// {
//     Console.WriteLine($"Exception: {ex.Message}");
// }
//
// try
// {
//     workTask.ChangeStatus(Status.InProgress);
// }
// catch (InvalidTaskOperationException ex)
// {
//     Console.WriteLine($"Exception : {ex.Message}");
//     
// }
// #endregion

#region CLI MENU
Console.WriteLine("-------------------------------HERE STARTS THE LOOPED MENU-----------------------------");
bool isAppRunning = true;
string welcomeText =
@"Welcome to the TaskFlow app console!
These are the available commands:
    1 - Add a project or a task
    2 - List the tasks for a corresponding project
    3 - Mark a task as complete
    4 - Search for a specific task
    5 - Help
    6 - Exit";

string helpText =
@"
These are the available commands : 
1 - Add a project or a task
2 - List the tasks for a project
3 - Mark a task as complete
4 - Search for a specific task
5 - Help
6 - Exit";


ProjectRepository projectsRepository = new ProjectRepository();
TaskRepository tasksRepository = new TaskRepository();

Console.WriteLine(welcomeText);
while (isAppRunning)
{
    Console.WriteLine("Enter a command number from 1-6. For help press 5.");
    string? input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            try
            {
                AddProjectOrTask();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        case "2":
            ListProjectTasks();
            break;
        case "3":
            MarkTaskComplete();
            break;
        case "4":
            await SearchTask();
            break;
        case "5":
            ShowHelp();
            break;
        case "6":
            isAppRunning = false;
            Console.WriteLine("Exiting app...");
            break;
        default:
            Console.WriteLine("Invalid input. Please enter a valid number between 1 and 6.");
            break;
    }
}

async void AddProjectOrTask()
{
    Console.WriteLine("Do you want to add a project or a task? (p/t)");
    string? choice = Console.ReadLine();

    if (choice?.ToLower() == "p")
    {
        Console.WriteLine("Enter project name :");
        string projectName = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter project description : ");
        string? projectDescription = Console.ReadLine();

        Project newProject = new Project(projectName, projectDescription);
        var resultProject = await projectsRepository.AddAsync(newProject); 
        
        if (resultProject.IsSuccess)
        {
            Console.WriteLine($"Project {newProject.Name} with ID : {newProject.ProjectCode} added successfully!");
        }
        else
        {
            Console.WriteLine(resultProject.Error);
        }
        
    }
    else if (choice?.ToLower() == "t")
    {
        var projectToAddTasksTo = await SelectProject();
        if (projectToAddTasksTo == null)
        {
            return; 
        }

        Console.WriteLine("Enter task title : ");
        string taskTitle = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter task description : ");
        string taskDescription = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter task status (New / In Progress / Completed) : ");
        Status? taskStatus = ParseStatus(Console.ReadLine());

        Console.WriteLine("Enter task due date (yyyy-MM-dd) : ");
        DueDate? taskDueDate = ParseDueDate(Console.ReadLine());

        Console.WriteLine("Enter task priority (Low / Medium / High) : ");
        Priority? taskPriority = ParsePriority(Console.ReadLine());

        Console.WriteLine("Is this task a personal task or a work task? (p/w)");
        string? taskType = Console.ReadLine();
        
        TaskItem newTask;

        if (taskType?.ToLower() == "w")
        {
            Console.WriteLine("Enter work task department : ");
            string taskDepartment = Console.ReadLine() ?? string.Empty;

            newTask = new WorkTask(title: taskTitle,
                                   description: taskDescription,
                                   dueDate: taskDueDate,
                                   department: taskDepartment,
                                   status: taskStatus ?? Status.New,
                                   priority: taskPriority ?? Priority.Low);

        }
        else 
        {
            Console.WriteLine("Enter personal task category : ");
            string taskCategory = Console.ReadLine() ?? string.Empty;

            newTask = new PersonalTask(title: taskTitle,
                                   description: taskDescription,
                                   dueDate: taskDueDate,
                                   category: taskCategory,
                                   status: taskStatus ?? Status.New,
                                   priority: taskPriority ?? Priority.Low);

        }
        
        var addTaskResult = await tasksRepository.AddAsync(newTask);
        if (addTaskResult.IsSuccess)
        {
            projectToAddTasksTo.AddTask(newTask);
            Console.WriteLine($"Task {newTask.Title} added to project {projectToAddTasksTo.Name} successfully!");
        }
        else
        {
            Console.WriteLine(addTaskResult.Error);
        }
    }
    else
    {
        throw new ArgumentException($"{choice} is not a valid option.");
    }
}

Priority? ParsePriority(string? input)
{
    if (string.IsNullOrWhiteSpace(input))
        return null;

    input = input.Trim();
    return Enum.TryParse<Priority>(input.Trim(), ignoreCase: true, out var value) ? value : null;
}

DueDate? ParseDueDate(string? input)
{
    if (string.IsNullOrWhiteSpace(input)) return null;
    if (DateTime.TryParse(input.Trim(), out var dt))
    {
        return new DueDate(dt);
    }
    return null; 
}

Status? ParseStatus(string? input)
{
    if (string.IsNullOrWhiteSpace(input))
        return null;

    input = input.Trim();
    
    if (input.Equals("in progress", StringComparison.OrdinalIgnoreCase))
    {
        input = "InProgress";
    }

    return Enum.TryParse<Status>(input, ignoreCase: true, out var value) ? value : null;
}

async Task<Project?> SelectProject()
{
    Console.WriteLine("Select a project by entering the ID");

    var projectListResult = await projectsRepository.GetAllAsync();
    if (!projectListResult.IsSuccess)
    {
        Console.WriteLine(projectListResult.Error);
        return null;
    }

    if (projectListResult.Value != null)
        foreach (Project p in projectListResult.Value)
        {
            Console.WriteLine($"Project {p.Name} with ID : {p.ProjectCode}");
        }

    string? projectId = Console.ReadLine()?.Trim();

    var projectToAddTasksToResult = await projectsRepository.GetByIdAsync(Guid.Parse(projectId!));

    if (projectToAddTasksToResult.IsSuccess)
    {
        return projectToAddTasksToResult.Value;
    }

    Console.WriteLine(projectToAddTasksToResult.Error);
    return null;

}

async void ListProjectTasks()
{
    var project = await SelectProject(); 
    if (project == null)
    {
        return;
    }
    project.ListTasks();
}

TaskItem? SelectTask(Project project)
{
    Console.WriteLine("Select a task by entering the ID");
    foreach (var t in project.Tasks) 
    {
        Console.WriteLine($"Task {t.Title} with ID : {t.TaskId}");
    }
    
    string? taskId = Console.ReadLine()?.Trim();

    TaskItem? taskToMarkCompleted = null;
    foreach (TaskItem t in project.Tasks) 
    {
        if (t.TaskId.ToString() == taskId)
        {
            taskToMarkCompleted = t;
        }
    }

    if (taskToMarkCompleted == null)
    {
        Console.WriteLine("Task not found");
    }
    
    return taskToMarkCompleted;
}

async void MarkTaskComplete()
{
    var project = await SelectProject(); 
    if (project == null)
    {
        return;
    }
    var task = SelectTask(project);
    if (task == null)
    {
        return;
    }
    project.CompleteTask(task);
}

async Task SearchTask()
{
    Console.WriteLine("By which criteria would you like to filter the tasks? (Status / Due date / Keyword)");
    string? filter = Console.ReadLine()?.Trim();

    if (!TryParseFilter(filter, out var filterType))
    {
        Console.WriteLine("Invalid filter type.");
        return;
    }

    void PrintResult<T>(Result<List<T>> result)
    {
        if (result.IsSuccess)
        {
            foreach (var task in result.Value)
            {
                Console.WriteLine(task);
            }
        }
        else
        {
            Console.WriteLine(result.Error);
        }
    }

    switch (filterType)
    {
        case "status":
            Console.WriteLine("Enter task status (New / In Progress / Completed) : ");
            Status? taskStatus = ParseStatus(Console.ReadLine());
            if (taskStatus == null)
            {
                Console.WriteLine("Invalid Status.");
                return;
            }
            var taskWithStatusResult = await tasksRepository.GetByStatusAsync(taskStatus.Value);
            PrintResult(taskWithStatusResult);
            break;
        
        case "due date":
            Console.WriteLine("Enter task due date (yyyy-MM-dd) : ");
            DueDate? taskDueDate = ParseDueDate(Console.ReadLine());
            if (taskDueDate == null)
            {
                Console.WriteLine("Invalid Due date.");
                return;
            }

            var taskWithDueDateResult = await tasksRepository.GetByDueDateAsync(taskDueDate);
            PrintResult(taskWithDueDateResult);
            break;
        
        case "keyword":
            Console.WriteLine("Enter keyword : ");
            string? taskKeyword = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(taskKeyword))
            {
                Console.WriteLine("Invalid keyword.");
                return;
            }

            var taskWithKeywordResult = await tasksRepository.GetByKeywordAsync(taskKeyword);
            PrintResult(taskWithKeywordResult);
            break;
    }
    
}

static bool TryParseFilter(string? input, out string? filterType)
{
    if (input == null)
    {
        filterType = null;
        return false;
    }

    if (input.Equals("Status", StringComparison.OrdinalIgnoreCase) || input.Equals("Due Date", StringComparison.OrdinalIgnoreCase) || input.Equals("Keyword", StringComparison.OrdinalIgnoreCase) )
    {
        filterType = input.ToLower();
        return true;
    }

    filterType = null;
    return false;

}

void ShowHelp()
{
    Console.WriteLine(helpText);
}
#endregion



#region testing repository pattern
// ///////////////////////Testing repository pattern
//
// IRepository<TaskItem, Guid> taskItemRepository = new TaskRepository();
// IRepository<Project, Guid> projectRepository = new ProjectRepository();
//
// var resultAddTask  = await taskItemRepository.AddAsync(personalTask);
// var resultAddProject =   projectRepository.AddAsync(testProject);
// var resultAddProjectJava =   projectRepository.AddAsync(testProjectJava);
//
// await Task.WhenAll(resultAddProject, resultAddProjectJava);
// Console.WriteLine("Both done");

//
// if (resultAddTask.IsSuccess)
// {
//     Console.WriteLine(resultAddTask.Value);
// }
//
// if (resultAddProject.IsSuccess)
// {
//     Console.WriteLine(resultAddProject.Value);
// }
// else
// {
//     Console.WriteLine(resultAddProject.Error);
// }


// await taskItemRepository.AddAsync(personalTask2);
// await taskItemRepository.AddAsync(personalTask3);
// await taskItemRepository.AddAsync(personalTask4);
// await taskItemRepository.AddAsync(workTask);

//taskItemRepository.UseLoadAsync();
//projectRepository.UseLoadAsync();

// var resultGetTask = await taskItemRepository.GetByIdAsync(personalTask.TaskId);
// if (resultGetTask.IsSuccess)
// {
//     Console.WriteLine(resultGetTask.Value);//this one has the object inside, but since i have overriden the ToString method, it will
//     //print the details
// }
//
// var resultGetProject = await projectRepository.GetByIdAsync(testProjectJava.ProjectCode);
// if (resultGetProject.IsSuccess)
// {
//     Console.WriteLine(resultGetProject.Value);
// }
// else
// {
//     Console.WriteLine(resultGetProject.Error);
// }
//
// await GetAllFunctionCall();
//
// await taskItemRepository.DeleteAsync(Guid.Parse("921ac595-8df8-4338-995c-22d945d34c37"));
//
// await GetAllFunctionCall();



 // async Task GetAllFunctionCall()
 // {
 //     Console.WriteLine("---------get all function call--------");
 //     var resultGetAll = await taskItemRepository.GetAllAsync();
 //     if (resultGetAll.IsSuccess)
 //     {
 //         if (resultGetAll.Value != null)
 //             foreach (var task in resultGetAll.Value)
 //             {
 //                 Console.WriteLine(task.GetType());
 //                 Console.WriteLine(task.ToString());
 //             }
 //     }
 //     else
 //     {
 //         Console.WriteLine(resultGetAll.Error);
 //     }
 // }

//testam update pe personal task
// Console.WriteLine("TESTING UPDATE ---------------------");
// var resultGetTask = await taskItemRepository.GetByIdAsync(Guid.Parse("b9f69219-7eea-498a-9f71-17f35015e228"));
// if (resultGetTask.IsSuccess)
// {
//     Console.WriteLine(resultGetTask.Value);
// }
//
// var copyOfPersonalTask = new PersonalTask(personalTask);
// copyOfPersonalTask.TaskId = Guid.Parse("b9f69219-7eea-498a-9f71-17f35015e228");
// copyOfPersonalTask.Title = "new title";
// copyOfPersonalTask.Description = null;
// copyOfPersonalTask.Status = Status.InProgress;
// copyOfPersonalTask.DueDate = null;
// copyOfPersonalTask.Priority = Priority.Low;
// copyOfPersonalTask.Category = null;
//
//
// var updateTaskResult = await taskItemRepository.UpdateAsync(copyOfPersonalTask);
// if (updateTaskResult.IsSuccess)
// {
//     Console.WriteLine(updateTaskResult.Value);
// }
// else
// {
//     Console.WriteLine(updateTaskResult.Error);
// }
//
// var resultGetTask1 = await taskItemRepository.GetByIdAsync(personalTask.TaskId);
// if (resultGetTask1.IsSuccess)
// {
//     Console.WriteLine(resultGetTask1.Value);//this one has the object inside, but since i have overriden the ToString method, it will
//     //print the details
// }

// Console.WriteLine("-----------TEST THE UPDATE FUNCTION-----------");
//
// personalTask.DisplayDetails();
// personalTask.Description = "Something else";
// taskItemRepository.Update(personalTask);
// personalTask.DisplayDetails();
//
//
// Console.WriteLine("---------TEST THE DELETE FUNCTION------------");
//
// taskItemRepository.Delete(personalTask.TaskId);
//
// var resultGet1 = taskItemRepository.Get(personalTask.TaskId);
// if (!resultGet1.IsSuccess)
// {
//     Console.WriteLine(resultGet1.Error);//this one has the object inside, but since i have overriden the ToString method, it will
//     //print the details
// }
#endregion