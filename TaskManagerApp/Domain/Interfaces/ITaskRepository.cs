
using Domain.Entities;

namespace Domain.Interfaces;
public interface ITaskRepository
{
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ctx);
    Task<TaskItem> AddAsync(TaskItem task, CancellationToken ctx);
    Task<TaskItem?> GetByIdAsync(int id, CancellationToken ctx);
    Task<TaskItem?> UpdateAsync(TaskItem task, CancellationToken ctx);
}
