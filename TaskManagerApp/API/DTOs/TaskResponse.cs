namespace API.DTOs;
public record TaskResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
