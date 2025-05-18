using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class ShiftControllerTest
{
    private readonly Mock<IShiftService> _mockService;
    private readonly ShiftController _controller;

    public ShiftControllerTest()
    {
        _mockService = new Mock<IShiftService>();
        _controller = new ShiftController(_mockService.Object);
    }

    private readonly WorkShift _testShift = new()
    {
        ShiftId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        StartTime = new TimeOnly(08, 30, 00),
        EndTime = new TimeOnly(17, 30, 00)
    };

    [Fact]
    public async Task AddWorkShift_ValidShift_ReturnsOk()
    {
        var result = await _controller.AddWorkShift(_testShift);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Shift successfully added", okResult.Value.ToString());
        
        _mockService.Verify(service => service.AddWorkShift(_testShift), Times.Once());
    }

    [Fact]
    public async Task AddWorkShift_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.AddWorkShift(_testShift)).Throws(new Exception());
        
        var result = await _controller.AddWorkShift(_testShift);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.AddWorkShift(_testShift), Times.Once());
    }
}
