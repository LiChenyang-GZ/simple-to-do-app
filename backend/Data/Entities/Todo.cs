using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Backend.Data.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}