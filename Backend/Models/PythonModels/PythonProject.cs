using System.Security.Cryptography;

namespace Backend.Models;

public class PythonProject
{
    public string name { get; set; }
    public int cost { get; set; }

    public List<string> categories { get; set; }

    public List<string> target { get; set; }
    
    public PythonProject() { }

    public PythonProject(string name, int cost, List<string> categories, List<string> target)
    {
        this.name = name;
        this.cost = cost;
        this.categories = categories;
        this.target = target;
    }
}