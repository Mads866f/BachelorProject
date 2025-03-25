namespace DTO.Models;

public class CreateElectionModel
{
    public required string Name { get; set; }
    public  required  int TotalBudget { get; set; }
    public required string Model { get; set; }
    public required string BallotDesign { get; set; }
    
    public CreateElectionModel() {}
}