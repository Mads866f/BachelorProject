namespace Backend.Models;

public class ScoresEntity
{
    public Guid VoterId { get; set; }
    public Guid ProjectId { get; set; }
    public int Grade { get; set; }
}