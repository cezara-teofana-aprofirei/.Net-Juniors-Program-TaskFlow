using System.Text.Json;
using System.Text.Json.Serialization;
using TaskFlowApp.Common;
using TaskFlowApp.Interfaces;

namespace TaskFlowApp.Models;

public abstract class Repository<T, TKey> : IRepository<T, TKey> where T : IIdentifiable<TKey>
{
    private readonly string _path;
    protected List<T> Items;

    public static JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    protected Repository()
    {
        Items = new List<T>();
        var fileName = typeof(T) switch
        {
            var t when t == typeof(TaskItem) => "tasks",
            var t when t == typeof(Project) => "projects",
            _ => default
        };
        
        _path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(),"Data"), $"{fileName}.json");
        Console.WriteLine(_path);
    }
    
    public async Task<List<T>> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_path))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            await File.WriteAllTextAsync(_path, "[]", ct);
        }

        await using var fs = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

        if (fs.Length == 0)
        {
            Items = new List<T>();
            return Items;
        }

        try
        {
            Items = (await JsonSerializer.DeserializeAsync<List<T>>(fs, JsonOptions, ct)) ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            throw;
        }

        Console.WriteLine($"Loaded {Items.Count} items");
        
        return Items;
    }
    
    public async Task PersistAsync(CancellationToken ct = default)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);

            await using var fs = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None, 4096,
                useAsync: true);

            await JsonSerializer.SerializeAsync(fs, (IEnumerable<T>)Items, JsonOptions, ct);
            await fs.FlushAsync(ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occured during persistance operation : {ex.Message}");
            throw;
        }
    }
    
    public abstract Task<Result<TKey>> AddAsync(T entity);
    
    public abstract Task<Result<T>> GetByIdAsync(TKey uniqueKey);
    
    public abstract Task<Result<List<T>>> GetAllAsync();
    
    public abstract Task<Result<T>> UpdateAsync(T entity);
    
    public abstract Task<Result<TKey>> DeleteAsync(TKey uniqueKey);
}