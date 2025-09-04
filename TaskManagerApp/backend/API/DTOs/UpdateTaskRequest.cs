namespace API.DTOs;
public record UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
}
