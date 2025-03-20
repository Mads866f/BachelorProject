using DTO.Models;

namespace Backend.Models
{
    public class ElectionEntity
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int TotalBudget { get; set; }
        public string Model { get; set; }
        public string BallotDesign { get; set; }

        // Constructor
        public ElectionEntity(Election electionDto)
        {
            Id = electionDto.id;  
            Name = electionDto.name;
            TotalBudget = electionDto.TotalBudget;  
            Model = electionDto.model;
            BallotDesign = electionDto.BallotDesign;
        }

        public ElectionEntity() { }

        // Method to convert to DTO
        public Election ToElectionDto()
        {
            return new Election()
            {
                id = Id ?? Guid.Empty,  // Default to Guid.Empty if Id is null
                name = Name,
                TotalBudget = TotalBudget,
                model = Model,
                BallotDesign = BallotDesign
            };
        }
    }
}