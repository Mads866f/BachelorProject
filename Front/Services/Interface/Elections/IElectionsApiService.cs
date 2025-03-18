using DTO.Models;

namespace Front.Services.Elections;

public interface IElectionsApiService
{
   Task<List<Election>> GetElections();
   
   Task<Election> GetElection(int id);
   
   Task<Election> CreateElection(Election election);
   
   Task<Election> UpdateElection(Election election);
   
   Task DeleteElection(int id);
}