namespace Backend.Models;

public class PythonProjects
{
    public required string Name { get; set; }
    public required int Cost { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Targets { get; set; }

    public PythonProjects()
    {
        Name = "";
        Cost = 0;
    }
}