namespace DTO.Models;

public class CreateProjectModel
{
    public required Guid ElectionId { get; set; }
    public required  string Name { get; set; }
    public required  int Cost { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Target>? Targets { get; set; }
}