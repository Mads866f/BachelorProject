using AutoMapper;
using DTO.Models;

namespace Front.Services.Elections;

public interface IProjectsApiService
{
    Task<List<Project>> GetProjectsWithProjectId(string id);
}