using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;

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

        public async Task<TodoDto> Handle(AddTodoCommand command, CancellationToken ct)
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

            var dto = await _db.Todos
                .Include(t => t.Category)
                .Where(t => t.Id == entity.Id)
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

            return dto!;
        }
    }
}