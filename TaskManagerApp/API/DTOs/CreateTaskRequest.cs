namespace API.DTOs;
public record CreateTaskRequest
{
    public string Name { get; set; } = string.Empty;
}
