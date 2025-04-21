using DTO.Models;

namespace Front.Services.Interface;

public interface IElectionsApiService
{
   Task<List<Election>> GetElections(string? status = null);
   
   Task<Election?> GetElection(Guid id);
   
   Task<Election> CreateElection(Election election);
   
   Task<Election> UpdateElection(Election election);
   
   Task DeleteElection(Guid id);
   
}