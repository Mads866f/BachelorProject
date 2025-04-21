namespace DTO.Models;

public class Project
{
    public Guid Id { get; set; }
    public required Guid ElectionId { get; set; }
    public required  string Name { get; set; }
    public required  int Cost { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Target>? Targets { get; set; }
    public int? votes { get; set; }
}