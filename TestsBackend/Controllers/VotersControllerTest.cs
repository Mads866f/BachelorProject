using Backend.Controllers.DataControllers;
using Backend.Services.Interfaces;
using DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;

namespace TestsBackend.Controllers
{
    public class VotersControllerTest
    {
        private readonly Mock<IVotersService> _mockService;
        private readonly Mock<IProjectService> _mockProjectService;
        private readonly Mock<ILogger<VotersController>> _mockLogger;
        private readonly VotersController _controller;

        public VotersControllerTest()
        {
            _mockService = new Mock<IVotersService>();
            _mockProjectService = new Mock<IProjectService>();
            _mockLogger = new Mock<ILogger<VotersController>>();
            _controller = new VotersController(_mockService.Object, _mockLogger.Object, _mockProjectService.Object);;
        }

        // Test for GetAllAsync
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenVotersExist()
        {
            // Arrange
            var mockVoters = new List<Voter> { new Voter { Id = Guid.NewGuid(), ElectionId = Guid.NewGuid() } };
            _mockService.Setup(service => service.GetAllVotersAsync())
                .ReturnsAsync(mockVoters);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Voter>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(mockVoters, okResult.Value);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNotFound_WhenNoVotersExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllVotersAsync())
                .ReturnsAsync(new List<Voter>());

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Voter>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Test for GetAsync
        [Fact]
        public async Task GetAsync_ReturnsOk_WhenVoterExists()
        {
            // Arrange
            var voterId = Guid.NewGuid();
            var mockVoter = new Voter { Id = voterId, ElectionId = Guid.NewGuid() };
            _mockService.Setup(service => service.GetVoterAsync(voterId))
                .ReturnsAsync(mockVoter);

            // Act
            var result = await _controller.GetAsync(voterId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(mockVoter, okResult.Value);
        }

        [Fact]
        public async Task GetAsync_ReturnsNotFound_WhenVoterDoesNotExist()
        {
            // Arrange
            var voterId = Guid.NewGuid();
            _mockService.Setup(service => service.GetVoterAsync(voterId))
                .ReturnsAsync((Voter)null);

            // Act
            var result = await _controller.GetAsync(voterId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        // Test for Create
        [Fact]
        public async Task Create_ReturnsOk_WhenVoterIsCreatedSuccessfully()
        {
            // Arrange
            var newVoter = new CreateVoter { ElectionId = Guid.NewGuid() };
            var createdVoter = new Voter { Id = Guid.NewGuid(), ElectionId = newVoter.ElectionId };
            _mockService.Setup(service => service.CreateVoterAsync(It.IsAny<CreateVoter>()))
                .ReturnsAsync(createdVoter);

            // Act
            var result = await _controller.Create(newVoter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result); 
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedVoter = Assert.IsType<Voter>(okResult.Value); 
            Assert.Equal(createdVoter.Id, returnedVoter.Id); 
        }


        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("ElectionId", "ElectionId is Invalid");
            var newVoter = new CreateVoter { ElectionId = Guid.NewGuid()};

            // Act
            var result = await _controller.Create(newVoter);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // Test for Update
        [Fact]
        public async Task Update_ReturnsOk_WhenVoterIsUpdatedSuccessfully()
        {
            // Arrange
            var voterToUpdate = new Voter { Id = Guid.NewGuid()};
            _mockService.Setup(service => service.UpdateVoterAsync(voterToUpdate))
                .ReturnsAsync(voterToUpdate);

            // Act
            var result = await _controller.Update(voterToUpdate);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(voterToUpdate, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var voterToUpdate = new Voter { Id = Guid.NewGuid()};
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Update(voterToUpdate);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        // Test for Delete
        [Fact]
        public async Task DeleteByIdAsync_ReturnsOk_WhenVoterIsDeletedSuccessfully()
        {
            // Arrange
            var voterId = Guid.NewGuid();
            _mockService.Setup(service => service.DeleteByIdAsync(voterId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteByIdAsync(voterId);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsNotFound_WhenVoterDoesNotExist()
        {
            // Arrange
            var voterId = Guid.NewGuid();
            _mockService.Setup(service => service.DeleteByIdAsync(voterId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteByIdAsync(voterId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test for exception handling in GetAllAsync
        [Fact]
        public async Task GetAllAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockService.Setup(service => service.GetAllVotersAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<Voter>>>(result);
            Assert.IsType<ObjectResult>(actionResult.Result);
            var objectResult = (ObjectResult)actionResult.Result;
            Assert.Equal(500, objectResult.StatusCode);
        }

        // Test for exception handling in Create
        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var newVoter = new CreateVoter { ElectionId = Guid.NewGuid()};
            _mockService.Setup(service => service.CreateVoterAsync(newVoter))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Create(newVoter);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Voter>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
