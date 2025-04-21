using System.Diagnostics.CodeAnalysis;

namespace DTO.Models;

public class Election
{
    public Election() { }

    [SetsRequiredMembers]
    public Election(string name, int totalBudget, string model, string ballotDesign)
    {
        Name = name;
        TotalBudget = totalBudget;
        Model = model;
        BallotDesign = ballotDesign;
    }

    public Guid Id { get; set; }
    public required string Name { get; set; }
    public  required  int TotalBudget { get; set; }
    public required string Model { get; set; }
    public required string BallotDesign { get; set; }
    public bool Ended { get; set; }

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