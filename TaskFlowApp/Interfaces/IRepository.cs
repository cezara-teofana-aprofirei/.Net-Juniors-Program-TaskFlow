using TaskFlowApp.Common;
using TaskFlowApp.Models;

namespace TaskFlowApp.Interfaces;

public interface IRepository<T, TKey> where T : IIdentifiable<TKey>
{
    Task<Result<TKey>> AddAsync(T entity);
    
    Task<Result<T>> GetByIdAsync(TKey uniqueKey);
    
    Task<Result<List<T>>> GetAllAsync();
    
    Task<Result<T>> UpdateAsync(T entity);
    
    Task<Result<TKey>> DeleteAsync(TKey uniqueKey);

}