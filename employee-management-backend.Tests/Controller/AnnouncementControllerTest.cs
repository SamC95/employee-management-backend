using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using static employee_management_backend.Tests.MockObjects.MockAnnouncements;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class AnnouncementControllerTest
{
    private readonly Mock<IAnnouncementService> _mockService;
    private readonly AnnouncementController _controller;
    private readonly Announcement _testAnnouncement;

    public AnnouncementControllerTest()
    {
        _mockService = new Mock<IAnnouncementService>();
        _controller = new AnnouncementController(_mockService.Object);
        _testAnnouncement = NewAnnouncementPost;
    }

    [Fact]
    public async Task CreateAnnouncementPost_ValidAnnouncement_ReturnsOk()
    {
        var result = await _controller.CreateAnnouncementPost(_testAnnouncement);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Announcement successfully created", okResult.Value.ToString());

        _mockService.Verify(service => service.CreateAnnouncementPost(_testAnnouncement), Times.Once());
    }

    [Fact]
    public async Task CreateAnnouncementPost_ArgumentException_ReturnsBadRequest()
    {
        _mockService.Setup(service => service.CreateAnnouncementPost(_testAnnouncement))
            .ThrowsAsync(new ArgumentException());

        var result = await _controller.CreateAnnouncementPost(_testAnnouncement);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.CreateAnnouncementPost(_testAnnouncement), Times.Once());
    }

    [Fact]
    public async Task CreateAnnouncementPost_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.CreateAnnouncementPost(_testAnnouncement)).ThrowsAsync(new Exception());
        
        var result = await _controller.CreateAnnouncementPost(_testAnnouncement);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.CreateAnnouncementPost(_testAnnouncement), Times.Once());
    }
}
