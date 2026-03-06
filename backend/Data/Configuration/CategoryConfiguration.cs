using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Modules.Category.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.Color).IsRequired().HasMaxLength(7);

            builder.HasData(
                new Category { Id = 1, Name = "Personal", Description = "Personal tasks", Color = "#60A5FA" },
                new Category { Id = 2, Name = "Work", Description = "Work related", Color = "#F97316" },
                new Category { Id = 3, Name = "Study", Description = "Learning and study", Color = "#34D399" }
            );
        }
    }
}