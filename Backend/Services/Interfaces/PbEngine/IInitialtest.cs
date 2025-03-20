using Backend.Models;

namespace Backend.Services.Interfaces.PbEngine;

public interface IInitialtest
{
   Task<string> Test();

   Task<List<PythonProjects>> TestElection();
}