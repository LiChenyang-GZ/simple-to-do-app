using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().HasData(
                new Todo { Id = 1, Text = "Buy groceries", Description = "Milk, eggs, bread", Completed = false },
                new Todo { Id = 2, Text = "Write report", Description = "Monthly sales report", Completed = false },
                new Todo { Id = 3, Text = "Walk dog", Description = "Evening walk", Completed = true }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}