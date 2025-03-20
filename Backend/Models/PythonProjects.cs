namespace Backend.Models;

public class PythonProjects
{
    public string name { get; set; }
    public int cost { get; set; }
    public List<string> categories { get; set; }
    public List<string> targets { get; set; }

    public PythonProjects() { }
}