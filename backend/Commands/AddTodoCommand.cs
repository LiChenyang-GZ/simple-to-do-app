using System.ComponentModel.DataAnnotations;
using Backend.Data;
using Backend.Data.Entities;

namespace Backend.Commands
{
    public class AddTodoCommand
    {
        [Required]
        public string Text { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
    }

    public class AddTodoCommandHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AddTodoCommandHandler> _logger;

        public AddTodoCommandHandler(AppDbContext db, ILogger<AddTodoCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<int> Handle(AddTodoCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Creating todo: {Text}", command.Text);

            var entity = new Todo
            {
                Text = command.Text,
                Description = command.Description,
                Completed = false,
                CategoryId = command.CategoryId
            };

            _db.Todos.Add(entity);
            await _db.SaveChangesAsync(ct);

            return entity.Id;
        }
    }
}