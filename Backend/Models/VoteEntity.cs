using DTO.Models;

namespace Backend.Models;

public class VoteEntity
{
    public Guid Id { get; set; }
    public Guid ElectionId { get; set; }
    public List<ScoresEntity> ScoresEntities { get; set; } = new();
}