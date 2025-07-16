using System.Security.Claims;
using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static employee_management_backend.Tests.MockObjects.MockAnnouncements;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class AnnouncementControllerTest
{
    private readonly Mock<IAnnouncementService> _mockService;
    private readonly AnnouncementController _controller;
    private readonly Announcement _testAnnouncement;
    private readonly Employee _testEmployee;

    public AnnouncementControllerTest()
    {
        _mockService = new Mock<IAnnouncementService>();
        _controller = new AnnouncementController(_mockService.Object);
        _testAnnouncement = NewAnnouncementPost;
        _testEmployee = TestEmployeeJohn;
    }

    private void SetUserContext(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
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

    [Fact]
    public async Task GetRecentAnnouncements_ReturnsOkResult_WithAnnouncements()
    {
        SetUserContext(_testEmployee.EmployeeId);

        var announcements = new List<Announcement> { _testAnnouncement };

        _mockService.Setup(service => service.GetRecentAnnouncements(_testEmployee.EmployeeId))
            .ReturnsAsync(announcements);

        var result = await _controller.GetRecentAnnouncements();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(announcements, okResult.Value);
    }

    [Fact]
    public async Task GetRecentAnnouncements_ReturnsBadRequest_OnArgumentException()
    {
        SetUserContext(_testEmployee.EmployeeId);

        _mockService.Setup(service => service.GetRecentAnnouncements(_testEmployee.EmployeeId))
            .ThrowsAsync(new ArgumentException("Invalid user"));

        var result = await _controller.GetRecentAnnouncements();

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        Assert.Contains("Invalid user", badRequestResult.Value.ToString());
    }

    [Fact]
    public async Task GetRecentAnnouncements_ReturnsServerError_OnException()
    {
        SetUserContext(_testEmployee.EmployeeId);

        _mockService.Setup(service => service.GetRecentAnnouncements(_testEmployee.EmployeeId))
            .ThrowsAsync(new Exception("Database error"));
        
        var result = await _controller.GetRecentAnnouncements();
        
        var serverErrorResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, serverErrorResult.StatusCode);
        Assert.NotNull(serverErrorResult.Value);
        Assert.Contains("An unexpected error occured", serverErrorResult.Value.ToString());
    }
}
