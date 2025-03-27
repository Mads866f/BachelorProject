namespace Backend.Models;

public class PythonProjects
{
    public required string name { get; set; }
    public required int cost { get; set; }
    public List<string>? categories { get; set; }
    public List<string>? targets { get; set; }

    public PythonProjects()
    {
        name = "";
        cost = 0;
    }
}