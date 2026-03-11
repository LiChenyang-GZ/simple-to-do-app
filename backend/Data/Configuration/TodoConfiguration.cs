using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.TaskItem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Data.Configuration
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Text).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Description).HasMaxLength(2000);
            builder.Property(t => t.UpdatedAt).IsRequired(false);
            builder.HasOne(t => t.Category)
                .WithMany(c => c.Todos)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(
                new Todo { Id = 1, Text = "Buy groceries", Description = "Milk, eggs, bread", Completed = false, CategoryId = 1 },
                new Todo { Id = 2, Text = "Write report", Description = "Monthly sales report", Completed = false, CategoryId = 2 },
                new Todo { Id = 3, Text = "Walk dog", Description = "Evening walk", Completed = true, CategoryId = 1 }
            );
        }
    }
}