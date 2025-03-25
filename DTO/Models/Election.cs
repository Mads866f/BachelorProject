namespace DTO.Models;

public class Election
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public  required  int TotalBudget { get; set; }
    public required string Model { get; set; }
    public required string BallotDesign { get; set; }

    public override string ToString()
    {
        return "Model:" +
            Id + ":" +
            Name + ":" +
            TotalBudget + ":" +
            Model + ":" +
            BallotDesign;
    }
    
}