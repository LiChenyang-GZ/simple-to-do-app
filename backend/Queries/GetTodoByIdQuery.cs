using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;

namespace Backend.Queries
{
    public class GetTodoByIdQuery
    {
        public int Id { get; set; }
    }

    public class GetTodoByIdQueryHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<GetTodoByIdQueryHandler> _logger;

        public GetTodoByIdQueryHandler(AppDbContext db, ILogger<GetTodoByIdQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TodoDto?> Handle(GetTodoByIdQuery query, CancellationToken ct)
        {
            _logger.LogInformation("Fetching todo {Id}", query.Id);

            return await _db.Todos
                .Include(t => t.Category)
                .Where(t => t.Id == query.Id)
                .Select(t => new TodoDto
                {
                    Id = t.Id,
                    Text = t.Text,
                    Description = t.Description,
                    Completed = t.Completed,
                    Category = t.Category == null ? null : new CategoryDto
                    {
                        Id = t.Category.Id,
                        Name = t.Category.Name,
                        Color = t.Category.Color
                    }
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}