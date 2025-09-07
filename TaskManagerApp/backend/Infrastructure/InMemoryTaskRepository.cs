using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure;
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _store = new();
    private int _currentId = 0;

    public Task<TaskItem> AddAsync(TaskItem task, CancellationToken ctx)
    {
        int id = Interlocked.Increment(ref _currentId);
        task.SetId(id);
        _store[id] = task;
        return Task.FromResult(task);
    }

    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ctx)
    {
        return Task.FromResult((IReadOnlyList < TaskItem > )_store.Values.OrderBy(x => x.Id).ToList());
    }

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ctx = default)
    {
        _store.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task<TaskItem?> UpdateAsync(TaskItem task, CancellationToken ctx = default)
    {
        if (!_store.ContainsKey(task.Id)) return Task.FromResult<TaskItem?>(null);
        _store[task.Id] = task;
        return Task.FromResult<TaskItem?>(task);
    }

    public Task<bool> DeleteByIdAsync(int id, CancellationToken ctx = default)
    {
        return Task.FromResult(_store.TryRemove(id, out _));
    }
}
