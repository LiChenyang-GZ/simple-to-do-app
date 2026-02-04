using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}