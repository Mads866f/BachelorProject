namespace DTO.Models;

public class Votes
{
    public Guid ElectionId { get; set; }   // ID of the election
    public Guid Id { get; set; }           // ID of the vote

    // Dictionary where Key = Vote Target (e.g., Project ID), Value = Vote Degree (e.g., Rank or Score)
    public Dictionary<Guid, int> VoteRecords { get; set; } = new();
}