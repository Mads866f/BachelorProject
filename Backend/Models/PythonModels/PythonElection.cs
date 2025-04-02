namespace Backend.Models;

public class PythonElection
{
    public int totalBudget { get; set; }
    public List<PythonProject> projects { get; set; }
    public List<PythonVoter> votes { get; set; }
    
    public PythonElection() {}
}