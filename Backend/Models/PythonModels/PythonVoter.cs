namespace Backend.Models;

public class PythonVoter
{
    public required List<string> selectedProjects { get; set; }
    public required List<int> selectedDegree { get; set; }

    public PythonVoter()
    {
        selectedProjects = new List<string>();
        selectedDegree = new List<int>();
    }

    public PythonVoter(List<string> selectedProjects, List<int> selectedDegree)
    {
        this.selectedProjects = selectedProjects;
        this.selectedDegree = selectedDegree;
    }
}