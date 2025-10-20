using TaskFlowApp.Enums;
using TaskFlowApp.Exceptions;
using TaskFlowApp.Models;
using TaskFlowApp.ValueObjects;

Project testProject = new Project("110721", ".NET practice project", "A project meant to develop .Net programming skills");

DueDate deadlinePersonalTask = new DueDate(DateTime.Today.AddDays(-1), true);
DueDate deadlinePersonalTask2 = new DueDate(DateTime.Today.AddDays(2), true);
DueDate deadlinePersonalTask3 = new DueDate(DateTime.Today.AddDays(5), true);
DueDate deadlineWorkTask = new DueDate(DateTime.Today.AddDays(3));

PersonalTask personalTask = new("Walk dog", null, deadlinePersonalTask, priority: Priority.Medium, category: "Pets");
PersonalTask taskNotInProject = new("Project unrelated task", "", deadlinePersonalTask, "Sports");

WorkTask workTask = new("Refactor code", "Refactor code for the console app", deadlineWorkTask, "Development", priority:Priority.High);

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
    Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate.Deadline}, priority : {task.Priority} ");
}
Console.WriteLine();

Console.WriteLine("This is the list with sorted tasks");
testProject.Tasks.Sort();
foreach (TaskItem task in testProject.Tasks)
{
    Console.WriteLine($"Task with title : {task.Title}, due date : {task.DueDate.Deadline}, priority : {task.Priority} ");
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
    workTask.changeStatus(Status.InProgress);
}
catch (InvalidTaskOperationException ex)
{
    Console.WriteLine($"Exception : {ex.Message}");
}



