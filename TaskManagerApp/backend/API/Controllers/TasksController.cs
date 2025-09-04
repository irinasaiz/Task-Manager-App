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
        var resp = tasks.Select(t => new TaskResponse { Id = t.Id, Name = t.Name });
        return Ok(resp);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetById(int id, CancellationToken ctx)
    {
        var task = (await _service.GetAllAsync(ctx)).FirstOrDefault(t => t.Id == id);
        if (task == null)
            return NotFound();
        var resp = new TaskResponse { Id = task.Id, Name = task.Name };
        return Ok(resp);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest req, CancellationToken ctx)
    {
        try
        {
            var t = await _service.CreateAsync(req.Name, ctx);
            var resp = new TaskResponse { Id = t.Id, Name = t.Name };
            return CreatedAtAction(nameof(GetById), new { id = t.Id }, resp);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails { Title = "Invalid input", Detail = ex.Message, Status = StatusCodes.Status400BadRequest });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponse>> Update(int id, [FromBody] UpdateTaskRequest req,
        CancellationToken ctx)
    {
        var updated = await _service.UpdateAsync(id, req.Title, ctx);
        if (updated is null) 
            return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ctx)
    {
        var deleted = await _service.DeleteAsync(id, ctx);
        if (deleted is false)
            return NotFound();
        return NoContent();
    }
}
