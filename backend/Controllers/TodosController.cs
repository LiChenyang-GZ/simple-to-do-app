using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _db;
    public TodosController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<TodoDto>>> GetAll()
    {
        var todos = await _db.Todos
        .Include(t => t.Category)
        .Select(t => new TodoDto {
            Id = t.Id,
            Text = t.Text,
            Description = t.Description,
            Completed = t.Completed,
            Category = t.Category == null ? null : new CategoryDto {
                Id = t.Category.Id,
                Name = t.Category.Name,
                Color = t.Category.Color
            }
        })
        .ToListAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoDto>> Get(int id)
    {
        var t = await _db.Todos
        .Include(x => x.Category)
        .Where(x => x.Id == id)
        .Select(x => new TodoDto {
            Id = x.Id,
            Text = x.Text,
            Description = x.Description,
            Completed = x.Completed,
            Category = x.Category == null ? null : new CategoryDto {
                Id = x.Category.Id,
                Name = x.Category.Name,
                Color = x.Category.Color
            }
        })
        .FirstOrDefaultAsync();
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