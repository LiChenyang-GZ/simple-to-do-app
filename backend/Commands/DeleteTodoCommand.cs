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

        public async Task<bool> Handle(DeleteTodoCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Deleting todo {Id}", command.Id);

            var entity = await _db.Todos.FindAsync(new object[] { command.Id }, ct);
            if (entity == null)
            {
                _logger.LogWarning("Todo {Id} not found", command.Id);
                return false;
            }

            _db.Todos.Remove(entity);
            await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Deleted todo {Id}", command.Id);
            return true;
        }
    }
}