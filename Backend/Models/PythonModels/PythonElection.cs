namespace Backend.Models;

public class PythonElection
{
    public int TotalBudget { get; set; }
    public List<PythonProject> Projects { get; set; }
    public List<PythonVoter> Votes { get; set; }
    
    public PythonElection() {}
}