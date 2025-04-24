namespace DTO.Models;

/// <summary>
/// Represents a group of voters with coherent preferences and associated metadata.
/// </summary>
public class CoherentVoterGroup
{
    /// <summary>
    /// Gets or sets the unique identifier for the cohesive group of voters.
    /// This identifier is used to differentiate between various voter groups
    /// and serves as a key for tracking their associated data and preferences.
    /// </summary>
    public Guid GroupId { get; set; }

    /// <summary>
    /// Gets or sets the list of projects associated with the voter group.
    /// Each project includes details such as its unique identifier, election ID, name, cost,
    /// and optionally associated categories and targets.
    /// </summary>
    public List<Project> Projects { get; set; } = [];

    /// <summary>
    /// Gets or sets the total number of voters in the group.
    /// This value represents the count of individual voters associated
    /// with a particular group and is used to determine the group's size.
    /// </summary>
    public int VotersInGroup { get; set; }

    /// <summary>
    /// Gets or sets the total budget allocated to the group of voters.
    /// This value represents the sum of financial resources available
    /// for the projects associated with this voter group.
    /// </summary>
    public int TotalBudget { get; set; }

    /// <summary>
    /// Gets or sets the proportion of voters in the group relative to the total number of voters.
    /// This property is used to determine the group's representation as a fraction
    /// of the entire voter population in the election.
    /// </summary>
    public int FractionOfTotalVoters { get; set; }

    // Used for display purposes in the frontend
    public bool ShowDetails { get; set; }
}