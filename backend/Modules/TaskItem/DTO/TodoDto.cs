using backend.Modules.Category.DTO;

namespace backend.Modules.TaskItem.DTO;

public class TodoDto
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public string? Description { get; set; }
    public bool Completed { get; set; }
    public CategoryDto? Category { get; set; }
}