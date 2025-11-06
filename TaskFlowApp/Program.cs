using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.Models;
using TaskFlowApp.ValueObjects;

Project testProject = new Project(".NET practice project", "A project meant to develop .Net programming skills");

DueDate deadlinePersonalTask = new DueDate(DateTime.Today.AddDays(-1), true);
DueDate deadlinePersonalTask2 = new DueDate(DateTime.Today.AddDays(2), true);
DueDate deadlinePersonalTask3 = new DueDate(DateTime.Today.AddDays(5), true);
DueDate deadlineWorkTask = new DueDate(DateTime.Today.AddDays(3));

PersonalTask personalTask = new("Walk dog", null, deadlinePersonalTask, priority: Priority.Medium, category: "Pets");
PersonalTask taskNotInProject = new("Project unrelated task", "", deadlinePersonalTask, "Sports");

WorkTask workTask = new("Refactor code", "Refactor code for the console app", deadlineWorkTask, "Development", priority: Priority.High);

testProject.AddTask(personalTask);
testProject.AddTask(workTask);

//method which lists all tasks from a project
Console.WriteLine("ListTasks function call:");
testProject.ListTasks();

//demonstrate complete task method
Console.WriteLine("Complete task function call");
testProject.CompleteTask(workTask);

//method to display details, behaves differently based on the task type
Console.WriteLine("DisplayDetails function call for tasks in Tasks list:");
foreach (TaskItem task in testProject.Tasks)
{
    task.DisplayDetails();
}

//demonstrating deduplication for tasks 
//the hashset uses the equals method from TaskItem to decide which tasks are unique
var copyOfPersonalTask1 = new PersonalTask(personalTask);
testProject.AddTask(copyOfPersonalTask1);

Console.WriteLine("This is the list with duplicated tasks");
foreach (TaskItem task in testProject.Tasks)
{
    Console.WriteLine($"Task with title : {task.Title} and id : {task.TaskId} ");
}
Console.WriteLine();

Console.WriteLine("This is the list with de-duplicated tasks");
var deduplicatedTasks = new HashSet<TaskItem>(testProject.Tasks);

foreach (TaskItem task in deduplicatedTasks)
{
    Console.WriteLine($"Task with title : {task.Title} and id : {task.TaskId} ");
}
Console.WriteLine();

//add some copies for the personalTask to demonstrate sorting
PersonalTask personalTask2 = new("Walk dog", null, deadlinePersonalTask2, priority: Priority.Medium, category: "Pets");
PersonalTask personalTask3 = new("Walk cat", null, deadlinePersonalTask3, priority: Priority.Medium, category: "Pets");
PersonalTask personalTask4 = new("Walk cat", null, deadlinePersonalTask3, priority: Priority.High, category: "Pets");

testProject.AddTask(personalTask2);
testProject.AddTask(personalTask3);
testProject.AddTask(personalTask4);

//demonstrate sorting for TaskItem objects
Console.WriteLine("This is the list with unsorted tasks");

foreach (TaskItem task in testProject.Tasks)
{
    Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate?.Deadline}, priority : {task.Priority} ");
}
Console.WriteLine();

Console.WriteLine("This is the list with sorted tasks");
testProject.Tasks.Sort();
foreach (TaskItem task in testProject.Tasks)
{
    Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate?.Deadline}, priority : {task.Priority} ");
}
Console.WriteLine();

//showcase ListOverdue and CompleteAllForProject methods
Console.WriteLine("Listing overdue projects");
testProject.ListOverdue();

//completing all projects
Console.WriteLine("Complete all tasks for project method call:");
testProject.CompleteAllForProject();

//listing all overdue again, the completed projects won't appear anymore
Console.WriteLine("Listing overdue projects after completing all");
testProject.ListOverdue();

//demonstrate custom exceptions
Console.WriteLine("Demonstrating custom exceptions:");
try
{
    testProject.CompleteTask(taskNotInProject);
}
catch (TaskNotFoundException ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}

try
{
    workTask.ChangeStatus(Status.InProgress);
}
catch (InvalidTaskOperationException ex)
{
    Console.WriteLine($"Exception : {ex.Message}");
}

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

List<Project> projects = new List<Project>();

projects.Add(testProject);

Console.WriteLine(welcomeText);
while (isAppRunning)
{
    Console.WriteLine("Enter a command number from 1-6. For help press 5.");
    string? input = Console.ReadLine()?.Trim();

    switch (input)
    {
        case "1":
            AddProjectOrTask();
            break;
        case "2":
            ListProjectTasks();
            break;
        case "3":
            MarkTaskComplete();
            break;
        case "4":
            SearchTask();
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

void AddProjectOrTask()
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
        projects.Add(newProject);
        Console.WriteLine($"Project {newProject.Name} with ID : {newProject.ProjectCode} added successfully!");
        return;
    }
    else if (choice?.ToLower() == "t")
    {
        var projectToAddTasksTo = SelectProject();
        if (projectToAddTasksTo == null)
        {
            return;
        }

        Console.WriteLine("Enter task title : ");
        string taskTitle = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter task description : ");
        string? taskDescription = Console.ReadLine();

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
        else //TODO :  add another case for invalid choice
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
        projectToAddTasksTo.AddTask(newTask);
        Console.WriteLine($"Task {newTask.Title} added to project {projectToAddTasksTo.Name} successfully!");
    }
}

Priority? ParsePriority(string? input)
{
    if (string.IsNullOrWhiteSpace(input))
        return null;

    input = input.Trim();
    //TODO: if try parse doesn't work, it should throw an error 
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

Project? SelectProject()
{
    if (projects.Count == 0)
    {
        Console.WriteLine("No projects available. Please add a project before creating a task.");
        return null;
    }

    Console.WriteLine("Select a project by entering the ID");
    foreach (Project p in projects)
    {
        Console.WriteLine($"Project {p.Name} with ID : {p.ProjectCode}");
    }

    string? projectId = Console.ReadLine()?.Trim();
    Project? projectToAddTasksTo = null;
    foreach (Project p in projects)
    {
        if (p.ProjectCode.ToString() == projectId)
        {
            projectToAddTasksTo = p;
        }
    }

    if (projectToAddTasksTo == null)
    {
        Console.WriteLine("Project not found");
    }

    return projectToAddTasksTo;
}

void ListProjectTasks()
{
    var project = SelectProject();
    if (project == null)
    {
        return;
    }
    project.ListTasks();
}

TaskItem? SelectTask(Project project)
{
    if (project.Tasks.Count == 0)
    {
        Console.WriteLine("No tasks available. Please create a task for this project first.");
        return null;
    }

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

void MarkTaskComplete()
{
    var project = SelectProject();
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

void SearchTask()
{
    //TODO in Phase 2
    Console.WriteLine("This functionality isn't available yet");
}

void ShowHelp()
{
    Console.WriteLine(helpText);
}