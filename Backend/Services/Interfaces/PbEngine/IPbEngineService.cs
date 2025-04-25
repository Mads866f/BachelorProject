using System.Runtime.CompilerServices;
using Backend.Models;
using DTO.Models;

namespace Backend.Services.Interfaces.PbEngine;

public interface IPbEngineService
{
   Task<List<PythonProjects>> CalculateElection(PythonElection election,int method,int ballotType);

   Task<PythonElection?> convert_real_election(string filepath);
   
   Task<Stream> DownloadElection(PythonElection election);

   Task<Dictionary<string, float>> GetAnalysisNumbers(PythonElection election, List<PythonProject> electedProjects);
   
}