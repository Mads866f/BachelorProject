using System.ComponentModel.DataAnnotations;

namespace DTO.Models;

public class Election
{
    public Guid? id { get; set; }
    public required string name { get; set; }
    public  required  int TotalBudget { get; set; }
    public required string model { get; set; }
    public string? JoinCode{ get; set; }
    public required string BallotDesign { get; set; }

    public override string ToString()
    {
        return "Model:" +
            id + ":" +
            name + ":" +
            TotalBudget + ":" +
            model + ":" +
            JoinCode + ":" +
            BallotDesign;
    }
    
}