namespace backend.Modules.TaskItem.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CategoryId { get; set; }
        public Category.Entities.Category? Category { get; set; }
    }
}