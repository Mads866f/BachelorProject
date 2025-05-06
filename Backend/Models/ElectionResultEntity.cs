namespace Backend.Models;

public class ElectionResultEntity
{
   public Guid Id { get; set; }
   public Guid ElectionId { get; set; }
   public string  UsedMethod { get; set; } = "";
   public string UsedBallot { get; set; } = "";
   public int TotalBudget { get; set; }
}