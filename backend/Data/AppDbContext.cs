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

        public DbSet<Todo> Todos  => Set<Todo>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Personal", Description = "Personal tasks", Color = "#60A5FA" },
                new Category { Id = 2, Name = "Work", Description = "Work related", Color = "#F97316" },
                new Category { Id = 3, Name = "Study", Description = "Learning and study", Color = "#34D399" }
            );

            modelBuilder.Entity<Todo>().HasData(
                new Todo { Id = 1, Text = "Buy groceries", Description = "Milk, eggs, bread", Completed = false, CategoryId = 1 },
                new Todo { Id = 2, Text = "Write report", Description = "Monthly sales report", Completed = false, CategoryId = 2 },
                new Todo { Id = 3, Text = "Walk dog", Description = "Evening walk", Completed = true, CategoryId = 1 }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}