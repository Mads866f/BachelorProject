namespace DTO.Models;

public class Project
{
    public Guid Id { get; set; }
    public required Guid ElectionId { get; set; }
    public required  string Name { get; set; }
    public required  int Cost { get; set; }
    public List<Category>? categories { get; set; }
    public List<Target>? targets { get; set; }
}