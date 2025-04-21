using DTO.Models;
using MudBlazor;

namespace Front.Components.ResultPage.CoherrentVoter;

public class CoherrentVoter
{
    public int number_of_voters {get; set; }
    public List<Project> projects {get; set; }

    public void increase_no_of_voters(int delta)
    {
        number_of_voters += delta;
    }

    public bool UpdateIfInGroup(List<Project> project_list)
    {
        foreach (var p in projects)
        {
            var pIsInProjects_list = project_list.Select(x => x.Id == p.Id).ToList().Any();
            if (!pIsInProjects_list)
            {
                return false;
            }
        }
        increase_no_of_voters(1);
        return true;
    }
}