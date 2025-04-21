namespace DTO.Models;

public class Scores
{
    public Guid Voter_Id { get; set; }
    public Guid Project_Id { get; set; }
    
    public Project? project { get; set; }
    public int Grade { get; set; }
}