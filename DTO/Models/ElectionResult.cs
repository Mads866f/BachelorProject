namespace DTO.Models;

/// <summary>
/// DTO Representing the result of a election
/// </summary>
public class ElectionResult
{
    /// <summary>
    /// The Unique identifier of the result calculation
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The unique identifier of the associated election
    /// </summary>
    public Guid ElectionId { get; set; }
    public List<Project> SubmittedProjects { get; set; } = [];
    public List<Project> ElectedProjects { get; set; } = [];
    public string UsedMethod { get; set; }
    public string UsedBallot { get; set; }

    public int TotalBudget  {get; set;} = 0;
    
    public override string ToString()
    {
        return $"Id:{Id}\n ElectionId: {ElectionId}\n SubmittedProjects: {SubmittedProjects.Count}\n ElectedProjects: {ElectedProjects.Count} \n UsedMethod: {UsedMethod}\n UsedBallot: {UsedBallot}\n TotalBudget: {TotalBudget}";
    }
}