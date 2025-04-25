namespace DTO.Models;

public class Project
{
    public Guid Id { get; set; }
    public required Guid ElectionId { get; set; }
    public required  string Name { get; set; }
    public required  int Cost { get; set; }
    public List<Category> Categories = [];
    public List<Target> Targets = [];
    public int? votes { get; set; }
}