namespace Backend.Models;

public class ScoresEntity
{
    public Guid Voter_Id { get; set; }
    public Guid Project_Id { get; set; }
    public ProjectsEntity? ProjectsEntity { get; set; }
    public int Grade { get; set; }
}