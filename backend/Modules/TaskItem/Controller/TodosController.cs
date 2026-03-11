using Microsoft.AspNetCore.Mvc;
using Backend.Modules.TaskItem.Commands;
using backend.Modules.TaskItem.DTO;
using Backend.Modules.TaskItem.Queries;


namespace Backend.Modules.TaskItem.Controller;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AddTodoCommandHandler _addHandler;
    private readonly UpdateTodoCommandHandler _updateHandler;
    private readonly DeleteTodoCommandHandler _deleteHandler;
    private readonly ToggleStatusCommandHandler _toggleStatusHandler;
    private readonly GetTodosQueryHandler _getAllHandler;
    private readonly GetTodoByIdQueryHandler _getByIdHandler;
    public TodosController(
        AddTodoCommandHandler addHandler,
        UpdateTodoCommandHandler updateHandler,
        DeleteTodoCommandHandler deleteHandler,
        ToggleStatusCommandHandler toggleStatusHandler,
        GetTodosQueryHandler getAllHandler,
        GetTodoByIdQueryHandler getByIdHandler)
    {
        _addHandler = addHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _toggleStatusHandler = toggleStatusHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>> GetAll(CancellationToken ct)
    {
        var todos = await _getAllHandler.Handle(new GetTodosQuery(), ct);
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> Get(int id, CancellationToken ct)
    {
        var todo = await _getByIdHandler.Handle(new GetTodoByIdQuery { Id = id }, ct);
        if (todo == null) return NotFound();
        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create([FromBody] AddTodoCommand command, CancellationToken ct)
    {
        var id = await _addHandler.Handle(command, ct);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        var success = await _updateHandler.Handle(command, ct);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deletedId = await _deleteHandler.Handle(new DeleteTodoCommand { Id = id }, ct);
        if (deletedId == null) return NotFound();
        return Ok(new { id = deletedId });
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<ActionResult<TodoDto>> ToggleStatus(int id, [FromQuery] bool isDone, CancellationToken ct)
    {
        var result = await _toggleStatusHandler.Handle(
            new ToggleStatusCommand { Id = id, IsDone = isDone }, ct);
        return Ok(result);
    }
}