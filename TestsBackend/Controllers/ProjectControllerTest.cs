using Backend.Controllers.DataControllers;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using Backend.Models;

namespace TestsBackend.Controllers
{
    public class ProjectControllerTest
    {
        private readonly Mock<IProjectService> _mockService;
        private readonly Mock<ILogger<ProjectController>> _mockLogger;
        private readonly ProjectController _controller;

        public ProjectControllerTest()
        {
            _mockService = new Mock<IProjectService>();
            _mockLogger = new Mock<ILogger<ProjectController>>();
            _controller = new ProjectController(_mockService.Object, _mockLogger.Object);
        }

        // Test for GetByElectionId
        [Fact]
        public async Task GetByElectionId_ReturnsOk_WhenProjectsExist()
        {
            // Arrange
            var electionId = Guid.NewGuid();
            var mockProjects = new List<Project> { new() { ElectionId = Guid.NewGuid(), Name = "TestProject",Cost = 100 } };
            _mockService.Setup(service => service.GetProjectsWithElectionId(It.IsAny<Guid>()))
                .ReturnsAsync(mockProjects);

            // Act
            var result = await _controller.GetByElectionId(electionId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Project>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(mockProjects, okResult.Value);
        }

        [Fact]
        public async Task GetByElectionId_ReturnsNotFound_WhenNoProjectsExist()
        {
            // Arrange
            var electionId = Guid.NewGuid();
            _mockService.Setup(service => service.GetProjectsWithElectionId(electionId))
                .ReturnsAsync(new List<Project>());

            // Act
            var result = await _controller.GetByElectionId(electionId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Project>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Test for CreateProject
        [Fact]
        public async Task CreateProject_ReturnsOk_WhenProjectIsCreatedSuccessfully()
        {
            // Arrange
            var newCreateProject = new CreateProjectModel() { ElectionId = Guid.NewGuid() ,Name = "New Project", Cost = 1000 };
            var createdProject = new Project
            {
                ElectionId = Guid.Empty,
                Name = "test",
                Cost = 0
            };
            _mockService.Setup(service => service.CreateProjectAsync(It.IsAny<CreateProjectModel>()))
                .ReturnsAsync(createdProject);

            // Act
            var result = await _controller.CreateProject(newCreateProject);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task CreateProject_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Name is required");
            var newCreateProject = new CreateProjectModel() { ElectionId = Guid.NewGuid() ,Name = "New Project", Cost = 1000 };

            // Act
            var result = await _controller.CreateProject(newCreateProject);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        // Test for UpdateProject
        [Fact]
        public async Task UpdateProject_ReturnsOk_WhenProjectIsUpdatedSuccessfully()
        {
            // Arrange
            var projectToUpdate = new ProjectsEntity { Id = Guid.NewGuid(), Name = "Updated Project" };
            _mockService.Setup(service => service.UpdateProjectAsync(It.IsAny<ProjectsEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProject(projectToUpdate);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateProject_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var projectToUpdate = new ProjectsEntity { Id = Guid.NewGuid(), Name = "" };
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.UpdateProject(projectToUpdate);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        // Test for DeleteProject
        [Fact]
        public async Task DeleteProject_ReturnsOk_WhenProjectIsDeletedSuccessfully()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _mockService.Setup(service => service.GetProjectByIdAsync(projectId))
                .ReturnsAsync(new Project
                {
                    Id = projectId,
                    ElectionId = Guid.NewGuid(),
                    Name = "test",
                    Cost = 10
                });
            _mockService.Setup(service => service.DeleteProjectAsync(projectId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            _mockService.Setup(service => service.GetProjectByIdAsync(projectId))
                .ReturnsAsync((Project)null!);

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test for exception handling in GetByElectionId
        [Fact]
        public async Task GetByElectionId_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var electionId = Guid.NewGuid();
            _mockService.Setup(service => service.GetProjectsWithElectionId(electionId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetByElectionId(electionId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Project>>>(result);
            Assert.IsType<ObjectResult>(actionResult.Result);
            var objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test for exception handling in CreateProject
        [Fact]
        public async Task CreateProject_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var newProject = new CreateProjectModel
            {
                Name = "New Project",
                ElectionId = Guid.NewGuid(),
                Cost = 0
            };
            _mockService.Setup(service => service.CreateProjectAsync(It.IsAny<CreateProjectModel>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.CreateProject(newProject);

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
            Assert.IsType<ObjectResult>(actionResult);
            var objectResult = (ObjectResult)actionResult;
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
