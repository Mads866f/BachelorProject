namespace DTO.Models;

public class Scores
{
    public Guid VoterId { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public int Grade { get; set; }
}