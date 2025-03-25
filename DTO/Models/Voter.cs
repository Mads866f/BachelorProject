namespace DTO.Models;

public class Voter
{
    public Guid Id { get; set; }
    public Guid ElectionId { get; set; }
    public List<Scores> Votes { get; set; } = [];
}

