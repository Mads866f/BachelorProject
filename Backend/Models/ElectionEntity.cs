using DTO.Models;

namespace Backend.Models
{
    public class ElectionEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalBudget { get; set; }
        public string Model { get; set; }
        public string BallotDesign { get; set; }
        public bool Ended { get; set; }
        public ElectionEntity() { }
        
        protected bool Equals(ElectionEntity other)
        {
            return Id.Equals(other.Id) 
                   && Name == other.Name 
                   && TotalBudget == other.TotalBudget 
                   && Model == other.Model 
                   && BallotDesign == other.BallotDesign;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, TotalBudget, Model, BallotDesign);
        }
    }
}