using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using Backend.Data;

namespace Backend.Commands
{
    public class DeleteTodoCommand
    {
        [Required]
        public int Id { get; set; }
    }

    public class DeleteTodoCommandHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<DeleteTodoCommandHandler> _logger;

        public DeleteTodoCommandHandler(AppDbContext db, ILogger<DeleteTodoCommandHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<int?> Handle(DeleteTodoCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Deleting todo {Id}", command.Id);

            var newTodo = await _db.Todos.FindAsync(command.Id, ct);
            if (newTodo == null)
            {
                _logger.LogWarning("Todo {Id} not found", command.Id);
                return null;
            }

            _db.Todos.Remove(newTodo);
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Deleted todo {Id}", command.Id);
            return newTodo.Id;
        }
    }
}