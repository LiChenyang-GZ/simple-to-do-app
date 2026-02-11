namespace Backend.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Color { get; set; }
    }

    public class TodoDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public CategoryDto? Category { get; set; }
    }
}