using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.DTOs;

namespace Backend.Queries
{
    public class GetTodosQuery { }

    public class GetTodosQueryHandler
    {
        private readonly AppDbContext _db;
        private readonly ILogger<GetTodosQueryHandler> _logger;

        public GetTodosQueryHandler(AppDbContext db, ILogger<GetTodosQueryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<TodoDto>> Handle(GetTodosQuery query, CancellationToken ct)
        {
            _logger.LogInformation("Fetching todos");

            return await _db.Todos
                .Include(t => t.Category)
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
                .ToListAsync(ct);
        }
    }
}