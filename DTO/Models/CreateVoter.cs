namespace DTO.Models;

public class CreateVoter
{
    public Guid ElectionId { get; set; }
    public List<Scores> Votes { get; set; } = [];
}