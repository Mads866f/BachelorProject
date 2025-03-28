namespace Backend.Models;

public class PythonVoter
{
    public required List<string> SelectedProjects { get; set; }
    public required List<int> SelectedDegree { get; set; }

    public PythonVoter()
    {
        SelectedProjects = new List<string>();
        SelectedDegree = new List<int>();
    }

    public PythonVoter(List<string> selectedProjects, List<int> selectedDegree)
    {
        SelectedProjects = selectedProjects;
        SelectedDegree = selectedDegree;
    }
}