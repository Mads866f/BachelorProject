using DTO.Models;

namespace Front.Components.ResultPage.CoherrentVoter;

public class CoherrentVoter
{
    public Guid id { get; set; }
    public int number_of_voters {get; set; }
    public int No_In_Group{get; set; }
    
    public List<Voter>  voters { get; set; }
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
        fraction = (int)((((double)number_of_voters) / No_In_Group)*100);
        
        return true;
    }

    public override bool Equals(object? obj) =>
        Equals(obj as CoherrentVoter);

    public bool Equals(CoherrentVoter? other)
    {
        if (other is null) 
            return false;
        if (ReferenceEquals(this, other)) 
            return true;

        return number_of_voters.Equals(other.number_of_voters)
               && projects.SequenceEqual(other.projects)
               && voters.Equals(other.voters);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            // start with a prime
            int hash = 17;
            hash = hash * 23 + number_of_voters.GetHashCode();

            // fold in each project
            foreach (var p in projects)
                hash = hash * 23 + p.GetHashCode();

            // fold in each voter
                hash = hash * 23 + voters.GetHashCode();

            return hash;
        }
    }

    public override string ToString()
    {
        return $"CoherrentVoter: " +
               $"Id={id}, " +
               $"NoOfVotersInElection={number_of_voters}, " +
               $"GroupSize={No_In_Group}, " +
               $"Fraction={fraction}, " +
               $"VoterCount={voters?.Count ?? 0}, " +
               $"ProjectNames=[{string.Join(", ", projects?.Select(p => p.Name) ?? Enumerable.Empty<string>())}], " +
               $"ShowDetails={ShowDetails}";
    }
}