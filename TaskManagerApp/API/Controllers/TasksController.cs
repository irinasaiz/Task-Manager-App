using API.DTOs;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _service;

    public TasksController(TaskService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAll(CancellationToken ctx)
    {
        var tasks = await _service.GetAllAsync(ctx);
        var resp = tasks.Select(t => new TaskResponse { Id = t.Id, Name = t.Name, IsCompleted = t.IsCompleted });
        return Ok(resp);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id, CancellationToken ctx)
    {
        var task = (await _service.GetAllAsync(ctx)).FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();
        var resp = new TaskResponse { Id = task.Id, Name = task.Name, IsCompleted = task.IsCompleted };
        return Ok(resp);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest req, CancellationToken ctx)
    {
        try
        {
            var t = await _service.CreateAsync(req.Name, ctx);
            var resp = new TaskResponse { Id = t.Id, Name = t.Name, IsCompleted = t.IsCompleted };
            return CreatedAtAction(nameof(GetById), new { id = t.Id }, resp);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails { Title = "Invalid input", Detail = ex.Message, Status = StatusCodes.Status400BadRequest });
        }
    }
    
}
