using TaskFlowApp;

Project testProject = new Project("110721", ".NET practice project", "A project meant to develop .Net programming skills");

PersonalTask personalTask = new(Guid.NewGuid(), "Walk dog", null, DateTime.Today.AddHours(5), priority: "Medium", category: "Pets");

WorkTask workTask = new(Guid.NewGuid(), "Refactor code", "Refactor code for the console app", DateTime.Today.AddDays(3), "Development", priority: "High");

testProject.AddTask(personalTask);
testProject.AddTask(workTask);

testProject.ListTasks();

testProject.CompleteTask(personalTask);

foreach (TaskItem task in testProject.Tasks)
{
    task.DisplayDetails();
}

