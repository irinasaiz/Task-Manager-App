using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Infrastructure;
public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentDictionary<int, TaskItem> _store = new();
    private int _nextId = 1;

    public Task<TaskItem> AddAsync(TaskItem task, CancellationToken ctx)
    {
        task.SetId(_nextId);
        _store[_nextId] = task;
        _nextId++; //TODO: increment thread safe
        return Task.FromResult(task);
    }

    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken ctx)
    {
        return Task.FromResult((IReadOnlyList < TaskItem > )_store.Values.OrderBy(x => x.Id).ToList());
    }

    public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
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
}
