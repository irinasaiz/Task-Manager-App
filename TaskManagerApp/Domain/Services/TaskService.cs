
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;
public class TaskService
{
    private readonly ITaskRepository _repo;

    public TaskService(ITaskRepository repo) => _repo = repo;

    public async Task<TaskItem> CreateAsync(string name, CancellationToken ctx = default)
    {
        var task = TaskItem.Create(name);
        return await _repo.AddAsync(task, ctx);
    }

    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ctx = default)
        => _repo.GetAllAsync(ctx);

    public async Task<TaskItem?> UpdateAsync(int id, string name, CancellationToken ctx = default)
    {
        TaskItem? existing = await _repo.GetByIdAsync(id, ctx);
        if (existing is null) 
            return null;

        existing.Rename(name);

        return await _repo.UpdateAsync(existing, ctx);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ctx)
    {
        bool existing = await _repo.DeleteByIdAsync(id, ctx);
        return existing;
    }
}
