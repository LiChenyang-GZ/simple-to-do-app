using System.ComponentModel.DataAnnotations;
using Backend.Data;

namespace Backend.Commands
{
    public class UpdateTodoCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = null!;
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public int? CategoryId { get; set; }
    }

    public class UpdateTodoCommandHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UpdateTodoCommandHandler> _logger;

        public UpdateTodoCommandHandler(AppDbContext db, ILogger<UpdateTodoCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateTodoCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Updating todo {Id}", command.Id);

            var updateTodo = await _db.Todos.FindAsync(command.Id, ct);
            if (updateTodo == null)
            {
                _logger.LogWarning("Todo {Id} not found", command.Id);
                return false;
            }

            updateTodo.Text = command.Text;
            updateTodo.Description = command.Description;
            updateTodo.Completed = command.Completed;
            updateTodo.CategoryId = command.CategoryId;

            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Updated todo {Id}", command.Id);
            return true;
        }
    }
}