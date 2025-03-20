namespace Backend.Models;

public class PythonElection
{
    public int TotalBudget { get; set; }
    public List<KeyValuePair<string, int>> Projects { get; set; }
    public List<List<KeyValuePair<string, int>>> Votes { get; set; }
    
    public PythonElection() {}
}