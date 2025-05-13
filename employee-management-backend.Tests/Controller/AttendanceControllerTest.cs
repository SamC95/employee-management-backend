using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class AttendanceControllerTest
{
    private readonly Mock<IAttendanceService> _mockService;
    private readonly AttendanceController _controller;

    public AttendanceControllerTest()
    {
        _mockService = new Mock<IAttendanceService>();
        _controller = new AttendanceController(_mockService.Object);
    }
    
    [Fact]
    public async Task PostClockEvent_ValidObject_ReturnsOk()
    {
        var clockEvent = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-in",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };

        var result = await _controller.PostClockEvent(clockEvent);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        Assert.NotNull(okResult.Value);
        Assert.Contains("Clock event saved successfully", okResult.Value.ToString());

        _mockService.Verify(service => service.PostClockEvent(It.IsAny<ClockEvent>()), Times.Once);
    }

    [Fact]
    public async Task PostClockEvent_InvalidObject_ReturnsBadRequest()
    {
        var clockEvent = new ClockEvent
        {
            ClockId = "",
            Type = "clock-in",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };

        var result = await _controller.PostClockEvent(clockEvent);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task PostClockEvent_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.PostClockEvent(It.IsAny<ClockEvent>())).Throws(new Exception());

        var clockEvent = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-in",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };
        
        var result = await _controller.PostClockEvent(clockEvent);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Error occurred", objectResult.Value.ToString());
        
        _mockService.Verify(service => service.PostClockEvent(It.IsAny<ClockEvent>()), Times.Once);
    }

    [Fact]
    public async Task GetClockEventsById_ValidId_ReturnsOk()
    {
        const string clockId = "1234567";

        var clockEvents = new List<ClockEvent>
        {
            new()
            {
                ClockId = clockId,
                Type = "clock-in",
                Timestamp = DateTime.Now,
            },
            new()
            {
                ClockId = clockId,
                Type = "clock-out",
                Timestamp = DateTime.Now,
            }
        };
        
        _mockService.Setup(service => service.GetClockEventsByClockId(clockId)).ReturnsAsync(clockEvents);
        
        var result = await _controller.GetClockEventsByClockId(clockId);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        
        var returnedList = (okResult.Value as IEnumerable<object>)!.ToList();
        Assert.Equal(2, returnedList.Count);
        
        _mockService.Verify(service => service.GetClockEventsByClockId(clockId), Times.Once);
    }

    [Fact]
    public async Task GetClockEventsById_EmptyResult_ReturnsEmptyList()
    {
        const string clockId = "9999999";
        
        _mockService.Setup(service => service.GetClockEventsByClockId(clockId)).ReturnsAsync(new List<ClockEvent>());
        
        var result = await _controller.GetClockEventsByClockId(clockId);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedList = (okResult.Value as IEnumerable<object>)!.ToList();
        Assert.Empty(returnedList);
        
        _mockService.Verify(service => service.GetClockEventsByClockId(clockId), Times.Once);
    }

    [Fact]
    public async Task GetClockEventsById_WhenServiceFails_ReturnsInternalServerError()
    {
        const string clockId = "1234567";
        
        _mockService.Setup(service => service.GetClockEventsByClockId(clockId)).Throws(new Exception());
        
        var result = await _controller.GetClockEventsByClockId(clockId);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Error occurred", objectResult.Value.ToString());
        
        _mockService.Verify(service => service.GetClockEventsByClockId(clockId), Times.Once);
    }
}
