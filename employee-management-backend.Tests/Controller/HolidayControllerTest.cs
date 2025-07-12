using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using static employee_management_backend.Tests.MockObjects.MockHolidayEvents;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class HolidayControllerTest
{
    private readonly Mock<IHolidayService> _mockService;
    private readonly HolidayController _holidayController;
    private readonly HolidayEvent _holidayEvent;

    public HolidayControllerTest()
    {
        _mockService = new Mock<IHolidayService>();
        _holidayController = new HolidayController(_mockService.Object);
        _holidayEvent = InProgressHolidayEvent;
    }

    [Fact]
    public async Task CreateHolidayRequest_ValidHolidayRequest_ReturnsOk()
    {
        var result = await _holidayController.CreateHolidayRequest(_holidayEvent);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Holiday request created successfully", okResult.Value.ToString());
        
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

    [Fact]
    public async Task UpdateHolidayStatus_WhenEventIdDoesNotMatch_ReturnsBadRequest()
    {
        var eventId = Guid.NewGuid();
        
        var result = await _holidayController.UpdateHolidayStatus(eventId, _holidayEvent);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.UpdateHolidayStatus(It.IsAny<HolidayEvent>()), Times.Never());
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenServiceReturnsFalse_ReturnsNotFound()
    {
        _mockService.Setup(service => service.UpdateHolidayStatus(_holidayEvent)).ReturnsAsync(false);
        
        var result = await _holidayController.UpdateHolidayStatus(_holidayEvent.EventId, _holidayEvent);
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.NotNull(notFoundResult);
        
        _mockService.Verify(service => service.UpdateHolidayStatus(_holidayEvent), Times.Once());
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenServiceReturnsTrue_ReturnsOk()
    {
        _mockService.Setup(service => service.UpdateHolidayStatus(_holidayEvent)).ReturnsAsync(true);
        
        var result = await _holidayController.UpdateHolidayStatus(_holidayEvent.EventId, _holidayEvent);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        
        _mockService.Verify(service => service.UpdateHolidayStatus(_holidayEvent), Times.Once());
    }
}
