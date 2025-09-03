namespace Domain.Entities;
public class TaskItem
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }

    public static TaskItem Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        return new TaskItem { Name = name.Trim(), IsCompleted = false};
    }

    public void SetId(int id) => Id = id;

    public void Rename(string name) => Name = name;

    public void Complete() => IsCompleted = true;
}
