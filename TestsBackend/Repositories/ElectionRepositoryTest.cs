using System.Data;
using Backend.Database;
using Backend.Models;
using Backend.Repositories;
using Backend.Utilities.TypeHandlers;
using Dapper;
using DTO.Models;
using Moq;
using TestsBackend.Database;

namespace TestsBackend.Repositories;

public class ElectionRepositoryTest
{
   private ElectionRepository _electionRepository;
   private SqliteDbFactory _dbConnectionFactory;

   public ElectionRepositoryTest()
   {
      SqlMapper.AddTypeHandler(new GuidTypeHandler());
      _dbConnectionFactory = new SqliteDbFactory();
      _dbConnectionFactory.InitializeDatabase();
      _electionRepository = new ElectionRepository((IDbConnectionFactory) _dbConnectionFactory);
   }

   public ElectionRepository GetElectionRepository()
   {
      _dbConnectionFactory = new SqliteDbFactory();
      _dbConnectionFactory.InitializeDatabase();
      _electionRepository = new ElectionRepository((IDbConnectionFactory) _dbConnectionFactory);
      return _electionRepository;
   }
   
   [Fact]
   public async Task GetAllAsync_NoParameters_EmptyDatabse()
   {
      //Arrange
      _electionRepository = GetElectionRepository();
      //Act
      var result = await _electionRepository.GetAllAsync();
      //Assert
      Assert.Empty(result);
   }

   [Fact]
   public async Task CreateAsync_NewElection_EmptyDatabase()
   {
      //Arrange
      _electionRepository = GetElectionRepository();
      var newElection = new Election { Name = "Test", TotalBudget = 20, Model = "EqualShares",BallotDesign = "1-Approval"};
      //Act
      var result = await _electionRepository.CreateAsync(newElection);
      //Assert
      Assert.NotNull(result);
      Assert.Equal("Test", result.Name);
      Assert.Equal(20, result.TotalBudget);
      Assert.Equal("1-Approval", result.BallotDesign);
      Assert.Equal("EqualShares", result.Model);
      Assert.IsType<ElectionEntity>(result);
   }

   [Fact]
   public async Task CreateAsync_ExistingElection_ExistingElection()
   {
      //Arrange
      _electionRepository = GetElectionRepository();
      var newElection = new Election { Name = "Test", TotalBudget = 20, Model = "EqualShares",BallotDesign = "1-Approval"};
      var firstInsert = await _electionRepository.CreateAsync(newElection);
      //Act
      var result = await _electionRepository.CreateAsync(newElection);
      //Assert
      Assert.NotNull(firstInsert);
      Assert.NotNull(result);
      Assert.Equal(firstInsert.Name, result.Name);
      Assert.NotEqual(firstInsert.Id, result.Id);
      Assert.IsType<ElectionEntity>(result);
   }

   [Fact]
   public async Task GetAllAsync_NoParameters_ExistingElection()
   {
      //Arrange
      _electionRepository = GetElectionRepository();
      var newElection1 = new Election { Name = "Test1", TotalBudget = 20, Model = "EqualShares",BallotDesign = "1-Approval"};
      var newElection2 = new Election { Name = "Test2", TotalBudget = 20, Model = "EqualShares",BallotDesign = "1-Approval"};
      var firstInsert = await _electionRepository.CreateAsync(newElection1);
      var secondInsert = await _electionRepository.CreateAsync(newElection2);
      //Act
      var result = await _electionRepository.GetAllAsync();
      //Assert
      Assert.Equal(firstInsert, result.First());
      Assert.Equal(secondInsert,result.Last());
      Assert.NotEqual(firstInsert, secondInsert);
   }

   [Fact]
   public async Task GetAsync_ExistingElection_ExistingElection()
   {
     //Arrange
     _electionRepository = GetElectionRepository();
     var newElection = new Election { Name = "Test1", TotalBudget = 20, Model = "EqualShares",BallotDesign = "1-Approval"};
     var insert = await _electionRepository.CreateAsync(newElection);
     //Act
     var result = await _electionRepository.GetByIdAsync(insert.Id);
     //Assert
     Assert.NotNull(result);
     Assert.Equal(insert.Name, result.Name);
     Assert.Equal(newElection.Name, result.Name);
   }

   [Fact]
   public async Task GetAsync_NotExistingElection_Null()
   {
      //Arrange
      _electionRepository = GetElectionRepository();
      //Act
      var result = await _electionRepository.GetByIdAsync(new Guid());
      //Assert
      Assert.Null(result);
   }
}