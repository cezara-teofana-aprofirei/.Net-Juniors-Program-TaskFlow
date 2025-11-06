using TaskFlowApp.Common;
using TaskFlowApp.Interfaces;

namespace TaskFlowApp.Models;

public class ProjectRepository : Repository<Project, Guid>
{
    public override async Task<Result<Guid>> AddAsync(Project entity)
    {
        try
        {
            await LoadAsync();
            Console.WriteLine("These are the items after loading the projects from the json file");
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
            
            if (Items.Any(t => t.ProjectCode == entity.ProjectCode))
            {
                return Result<Guid>.Failure("Project already exists!");
            }
            
            Console.WriteLine("These are the items after adding the project to the list");
            Items.Add((entity));
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
            await PersistAsync();
            return Result<Guid>.Success(entity.ProjectCode);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<Project>> GetByIdAsync(Guid uniqueKey)
    {
        try
        {
            await LoadAsync();
            var result = Items.SingleOrDefault(t => t.ProjectCode == uniqueKey);
            return result is null
                ? Result<Project>.Failure($"Project with id : {uniqueKey} not found.")
                : Result<Project>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<Project>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<List<Project>>> GetAllAsync()
    {
        try
        {
            await LoadAsync();
            return Result<List<Project>>.Success(new List<Project>(Items));
        }
        catch (Exception ex)
        {
            return Result<List<Project>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<Project>> UpdateAsync(Project entity)
    {
        try
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await LoadAsync();
            var index = Items.FindIndex(t => t.Equals(entity));
            if (index < 0)
            {
                return Result<Project>.Failure($"Project with id : {entity.ProjectCode} not found.");
            }

            var current = Items[index];

            current.Name = entity.Name;
            current.Description = entity.Description;
            foreach (var task in entity.Tasks)
            {
                current.AddTask(task);
            }

            await PersistAsync();
            return Result<Project>.Success(current);
        }
        catch (Exception ex)
        {
            return Result<Project>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<Guid>> DeleteAsync(Guid uniqueKey)
    {
        try
        {
            await LoadAsync();
            var project = Items.FirstOrDefault(t => t.ProjectCode == uniqueKey);
            if (project is null)
            {
                return Result<Guid>.Failure($"TaskItem with Id : {uniqueKey} doesn't exist.");
            }

            Items.Remove(project);
            await PersistAsync();
            return Result<Guid>.Success(uniqueKey);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
    
}