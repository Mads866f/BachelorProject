using System.Runtime.InteropServices;

namespace Backend.Models;

public class PythonElection
{
    public int totalBudget { get; set; }
    public string? method {get;set;}
    public string? ballot_type {get;set;}
    public string? name {get;set;}
    public List<PythonProject> projects { get; set; }
    public List<PythonVoter> votes { get; set; }
    public PythonElection() {}
    
}