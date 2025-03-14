namespace Backend.Services.Interfaces.PbEngine

public interface ICalcElection
{
    Task<string> CalculateElection(string instance,string profile);
}