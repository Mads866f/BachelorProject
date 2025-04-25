using DTO.Models;

namespace Front.Components.ResultPage.CoherrentVoter;

public class CoherrentVoter
{
    public Guid id { get; set; }
    public int number_of_voters {get; set; }
    public int voters {get; set; }
    public int fraction {get; set; }
    public List<Project> projects {get; set; }
    
    public bool ShowDetails { get; set; }

    public CoherrentVoter()
    {
        id = Guid.NewGuid();
    }

    public void increase_no_of_voters(int delta)
    {
        number_of_voters += delta;
    }

    public bool UpdateIfInGroup(List<Project> project_list)
    {
        if (project_list.Count != projects.Count)
        {
            return false;
        }
        foreach (var p in projects)
        {
            var id_list = project_list.Select(x => x.Id).ToList();
            var pIsInProjects_list = id_list.Contains(p.Id);
            if (!pIsInProjects_list)
            {
                return false;
            }
        }
        increase_no_of_voters(1);
        fraction = (int)((((double)number_of_voters) / voters)*100);
        
        return true;
    }
}