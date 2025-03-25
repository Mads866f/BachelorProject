using System.Security.Cryptography;

namespace Backend.Models;

public class PythonProject
{
    public string Name { get; set; }
    public int Cost { get; set; }

    public List<string> Categories { get; set; }

    public List<string> Target { get; set; }
    
    public PythonProject() { }
}