using Backend.Models;

namespace Backend.Services.Interfaces.PbEngine;

public interface IPbEngineService
{
   Task<List<PythonProjects>> CalculateElection(PythonElection election,int method,int ballotType);
}