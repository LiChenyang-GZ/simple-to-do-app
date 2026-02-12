using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;
using Backend.Commands;
using Backend.Queries;
using Backend;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AddTodoCommandHandler _addHandler;
    private readonly UpdateTodoCommandHandler _updateHandler;
    private readonly DeleteTodoCommandHandler _deleteHandler;
    private readonly GetTodosQueryHandler _getAllHandler;
    private readonly GetTodoByIdQueryHandler _getByIdHandler;
    public TodosController(
        AddTodoCommandHandler addHandler,
        UpdateTodoCommandHandler updateHandler,
        DeleteTodoCommandHandler deleteHandler,
        GetTodosQueryHandler getAllHandler,
        GetTodoByIdQueryHandler getByIdHandler)
    {
        _addHandler = addHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
    }

    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>> GetAll()
    {
        var todos = await _getAllHandler.Handle(new GetTodosQuery(), CancellationToken.None);
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> Get(int id)
    {
        var todo = await _getByIdHandler.Handle(new GetTodoByIdQuery { Id = id }, CancellationToken.None);
        if (todo == null) return NotFound();
        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> Create([FromBody] AddTodoCommand command, CancellationToken ct)
    {
        var created = await _addHandler.Handle(command, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        var ok = await _updateHandler.Handle(command, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _deleteHandler.Handle(new DeleteTodoCommand { Id = id }, ct);
        return ok ? NoContent() : NotFound();
    }
        
}