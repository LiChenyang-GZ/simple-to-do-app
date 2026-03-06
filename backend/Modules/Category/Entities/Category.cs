using backend.Modules.TaskItem.Entities;

namespace backend.Modules.Category.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Color { get; set; }
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}