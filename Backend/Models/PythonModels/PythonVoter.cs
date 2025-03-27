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
}