using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _db;
    public TodosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<Todo>>> GetAll()
    {
        var list = await _db.Todos.ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> Get(int id)
    {
        var t = await _db.Todos.FindAsync(id);
        if (t is null) return NotFound();
        return t;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Create(Todo todo)
    {
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Todo todo) {
        if (id != todo.Id) return BadRequest();
        _db.Entry(todo).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var t = await _db.Todos.FindAsync(id);
        if (t is null) return NotFound();
        _db.Todos.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}