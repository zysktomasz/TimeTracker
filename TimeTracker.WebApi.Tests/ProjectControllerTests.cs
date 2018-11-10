using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using TimeTracker.Services.Interfaces;
using TimeTracker.WebApi.Controllers;
using Xunit;

namespace TimeTracker.WebApi.Tests
{
    public class ProjectControllerTests
    {
        [Fact]
        public void GetProjectById_ExistingIdPassed_ReturnsOkResult()
        {
            // arrange
            var serviceMock = new Mock<IProjectService>();
            serviceMock
                .Setup(s => s.GetProjectById(5))
                .Returns(new Services.DTO.Project.ProjectDto { ProjectID = 5, Name = "Whatever" });
            var controller = new ProjectController(serviceMock.Object);

            // act
            var result = controller.GetProjectById(5);

            // assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
