namespace Backend.Models;

public class ElectionResultEntity
{
   public Guid Id { get; set; }
   public Guid ElectionId { get; set; }
   public string MethodUsed { get; set; } = "";
   public string BallotUsed { get; set; } = "";
   public int TotalBudget { get; set; }
}