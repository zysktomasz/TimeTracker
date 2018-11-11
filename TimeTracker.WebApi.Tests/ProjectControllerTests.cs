using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using TimeTracker.Services.DTO.Project;
using TimeTracker.Services.Interfaces;
using TimeTracker.WebApi.Controllers;
using Xunit;

namespace TimeTracker.WebApi.Tests
{
    public class ProjectControllerTests
    {
        private Mock<IProjectService> _serviceMock;

        public ProjectControllerTests()
        {
            _serviceMock = new Mock<IProjectService>();
        }

        
        [Fact]
        public void GetProjectById_ExistingIdPassed_ReturnsOkResult()
        {
            // arrange
            var exampleProject = new ProjectDto { ProjectID = 5, Name = "Whatever" };
            _serviceMock
                .Setup(s => s.GetProjectById(5))
                .Returns(exampleProject);
            var controller = new ProjectController(_serviceMock.Object);

            // act
            var result = controller.GetProjectById(5);

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetProjectById_ExistingIdPassed_ReturnsRightItem()
        {
            // arrange
            var exampleProject = new ProjectDto { ProjectID = 5, Name = "Whatever" };
            _serviceMock
                .Setup(s => s.GetProjectById(5))
                .Returns(exampleProject);
            var controller = new ProjectController(_serviceMock.Object);

            // act
            var okResult = controller.GetProjectById(5) as OkObjectResult;

            // assert
            Assert.IsType<ProjectDto>(okResult.Value);
            Assert.Equal(exampleProject.Name, (okResult.Value as ProjectDto).Name);
        }

        // not sure if this even makes sense
        [Fact]
        public void GetProjectById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // arrange
            _serviceMock
                .Setup(s => s.GetProjectById(666));
            var controller = new ProjectController(_serviceMock.Object);

            // act
            var result = controller.GetProjectById(666);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public void GetAllProjects_WhenCalled_ReturnsOkResult()
        {
            // arrange
            var controller = new ProjectController(_serviceMock.Object);

            // act
            var result = controller.GetAllProjects();

            // assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllProjects_WhenCalled_ReturnsAllProjects()
        {
            // arrange
            _serviceMock
                .Setup(s => s.GetAllProjects())
                .Returns(new List<ProjectDto>
                {
                    new ProjectDto { ProjectID = 1, Name = "Chilling" },
                    new ProjectDto { ProjectID = 2, Name = "Reading" },
                    new ProjectDto { ProjectID = 3, Name = "Coding" }
                });
            var controller = new ProjectController(_serviceMock.Object);

            // act
            var result = controller.GetAllProjects();
            var okResult = result as OkObjectResult;

            // assert
            var projectsReturned = Assert.IsType<List<ProjectDto>>(okResult.Value);
            Assert.Equal(3, projectsReturned.Count);
        }


    }
}
