namespace Backend.Models;

public class ElectionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int TotalBudget { get; set; }
    public string Model { get; set; }
    public string JoinCode { get; set; }
    public string BallotDesign { get; set; }

}