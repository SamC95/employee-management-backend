using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class HolidayControllerTest
{
    private readonly Mock<IHolidayService> _mockService;
    private readonly HolidayController _holidayController;

    public HolidayControllerTest()
    {
        _mockService = new Mock<IHolidayService>();
        _holidayController = new HolidayController(_mockService.Object);
    }
    
    private readonly HolidayEvent _holidayEvent = new()
    {
        EventId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        Status = "in-progress",
    };

    [Fact]
    public async Task CreateHolidayRequest_ValidHolidayRequest_ReturnsOk()
    {
        var result = await _holidayController.CreateHolidayRequest(_holidayEvent);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Holiday request created", okResult.Value.ToString());
        
        _mockService.Verify(service => service.CreateHolidayRequest(_holidayEvent), Times.Once());
    }

    [Fact]
    public async Task CreateHolidayRequest_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.CreateHolidayRequest(_holidayEvent)).Throws(new Exception());
        
        var result = await _holidayController.CreateHolidayRequest(_holidayEvent);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.CreateHolidayRequest(_holidayEvent), Times.Once());
    }

    [Fact]
    public async Task CreateHolidayRequest_WhenStatusIsEmpty_ReturnsBadRequest()
    {
        _holidayEvent.Status = "";
        
        var result = await _holidayController.CreateHolidayRequest(_holidayEvent);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.CreateHolidayRequest(_holidayEvent), Times.Never());
    }
    
    [Fact]
    public async Task CreateHolidayRequest_WhenStatusIsNull_ReturnsBadRequest()
    {
        _holidayEvent.Status = _holidayEvent.Status = null;
        
        var result = await _holidayController.CreateHolidayRequest(_holidayEvent);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.CreateHolidayRequest(_holidayEvent), Times.Never());
    }
}
