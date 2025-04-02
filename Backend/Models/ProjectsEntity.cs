namespace Backend.Models;

public class ProjectsEntity
{
    public Guid Id { get; set; }
    public Guid ElectionId { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
}