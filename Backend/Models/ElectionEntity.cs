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

        public ElectionEntity()
        {
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ElectionEntity other)
                return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   BallotDesign == other.BallotDesign &&
                   Model == other.Model &&
                   TotalBudget == other.TotalBudget;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, BallotDesign, Model, TotalBudget);
        }

    }
}