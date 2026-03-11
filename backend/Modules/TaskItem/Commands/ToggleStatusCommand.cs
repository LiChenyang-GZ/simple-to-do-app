using Backend.Data;
using Backend.Shared.Exceptions;
using backend.Modules.TaskItem.DTO;
using backend.Modules.TaskItem.Entities;
using backend.Modules.Category.DTO;
using Microsoft.EntityFrameworkCore;

namespace Backend.Modules.TaskItem.Commands;

public class ToggleStatusCommand
{
    public int Id { get; set; }
    public bool IsDone { get; set; }
}

public class ToggleStatusCommandHandler
{
    private readonly AppDbContext _db;
    private readonly ILogger<ToggleStatusCommandHandler> _logger;

    public ToggleStatusCommandHandler(AppDbContext db, ILogger<ToggleStatusCommandHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<TodoDto> Handle(ToggleStatusCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Toggling status for todo {Id} to {IsDone}", command.Id, command.IsDone);

        var todo = await _db.Todos
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == command.Id, ct);

        if (todo is null)
        {
            throw new NotFoundException(nameof(Todo), command.Id);
        }

        if (todo.Completed == command.IsDone)
        {
            throw new ConflictException(
                $"Todo {command.Id} is already {(command.IsDone ? "completed" : "not completed")}.");
        }

        todo.Completed = command.IsDone;
        todo.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Toggled todo {Id} to Completed={IsDone}", command.Id, command.IsDone);

        return new TodoDto
        {
            Id = todo.Id,
            Text = todo.Text,
            Description = todo.Description,
            Completed = todo.Completed,
            Category = todo.Category is null ? null : new CategoryDto
            {
                Id = todo.Category.Id,
                Name = todo.Category.Name,
                Color = todo.Category.Color
            }
        };
    }
}
