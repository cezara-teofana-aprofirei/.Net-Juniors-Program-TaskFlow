using System.Text.Json.Serialization;
using TaskFlowApp.Common;
using TaskFlowApp.Enums;
using TaskFlowApp.Interfaces;
using TaskFlowApp.ValueObjects;

namespace TaskFlowApp.Models;

public class TaskRepository : Repository<TaskItem, Guid>
{
    public override async Task<Result<Guid>> AddAsync(TaskItem entity)
    {
        try
        {
            await LoadAsync();
            if (Items.Any(t => t.TaskId == entity.TaskId))
            {
                return Result<Guid>.Failure("Task already exists!");
            }

            Items.Add((entity));
            await PersistAsync();
            return Result<Guid>.Success(entity.TaskId);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
    
    public override async Task<Result<TaskItem>> GetByIdAsync(Guid uniqueKey)
    {
        try
        {
            await LoadAsync();
            var result = Items.SingleOrDefault(t => t.TaskId == uniqueKey);
            return result is null
                ? Result<TaskItem>.Failure($"Task with id : {uniqueKey} not found.")
                : Result<TaskItem>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<TaskItem>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
    
    public override async Task<Result<List<TaskItem>>> GetAllAsync()
    {
        try
        {
            await LoadAsync();
            return Result<List<TaskItem>>.Success(new List<TaskItem>(Items));
        }
        catch (Exception ex)
        {
            return Result<List<TaskItem>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<TaskItem>> UpdateAsync(TaskItem entity)
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
                return Result<TaskItem>.Failure($"TaskItem with Id : {entity.TaskId} doesn't exist.");
            }

            var current = Items[index];

            if (current.GetType() != entity.GetType())
            {
                return Result<TaskItem>.Failure(
                    $"Cannot change task type from {Items[index].GetType()} to {entity.GetType()}");
            }

            current.Title = entity.Title;
            current.Description = entity.Description;
            current.DueDate = entity.DueDate;
            current.Priority = entity.Priority;
            current.Status = entity.Status;
            current.CopySpecificType(entity);

            await PersistAsync();
            return Result<TaskItem>.Success(current);
        }
        catch (Exception ex)
        {
            return Result<TaskItem>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public override async Task<Result<Guid>> DeleteAsync(Guid uniqueKey)
    {
        try
        {
            await LoadAsync();
            var task = Items.FirstOrDefault(t => t.TaskId == uniqueKey);
            if (task is null)
            {
                return Result<Guid>.Failure("TaskItem with Id : {uniqueKey} doesn't exist.");
            }

            Items.Remove(task);
            await PersistAsync();
            return Result<Guid>.Success(uniqueKey);
            
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public async Task<Result<List<TaskItem>>> GetByStatusAsync(Status status)
    {
        try
        {
            await LoadAsync();
            var result = Items.Where(t => t.Status == status).ToList();

            return result.Any()
                ? Result<List<TaskItem>>.Success(result)
                : Result<List<TaskItem>>.Failure($"No tasks found with status : {status}");
        }
        catch (Exception ex)
        {
            return Result<List<TaskItem>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
    
    public async Task<Result<List<TaskItem>>> GetByPriority(Priority priority)
    {
        try
        {
            await LoadAsync();
            var result = Items.Where(t => t.Priority == priority).ToList();

            return result.Any()
                ? Result<List<TaskItem>>.Success(result)
                : Result<List<TaskItem>>.Failure($"No tasks found with priority : {priority}");
        }
        catch (Exception ex)
        {
            return Result<List<TaskItem>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
    
    public async Task<Result<List<TaskItem>>> GetByDueDateAsync(DueDate dueDate)
    {
        try
        {
            await LoadAsync();
            var result = Items.Where(t => t.DueDate == dueDate).ToList();

            return result.Any()
                ? Result<List<TaskItem>>.Success(result)
                : Result<List<TaskItem>>.Failure($"No tasks found with due date : {dueDate}");
        }
        catch (Exception ex)
        {
            return Result<List<TaskItem>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }

    public async Task<Result<List<TaskItem>>> GetByKeywordAsync(string keyWord)
    {
        try
        {
            await LoadAsync();
            var result = Items.Where(t =>
                    t.Title.Contains(keyWord, StringComparison.OrdinalIgnoreCase) ||
                    t.Description.Contains(keyWord, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return result.Any()
                ? Result<List<TaskItem>>.Success(result)
                : Result<List<TaskItem>>.Failure($"No tasks found with keyword : {keyWord}");
        }
        catch (Exception ex)
        {
            return Result<List<TaskItem>>.Failure($"Error occured during serialization/deserialization : {ex.Message}");
        }
    }
}