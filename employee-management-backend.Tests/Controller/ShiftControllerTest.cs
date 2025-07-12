using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using static employee_management_backend.Tests.MockObjects.MockWorkShifts;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class ShiftControllerTest
{
    private readonly Mock<IShiftService> _mockService;
    private readonly ShiftController _controller;
    private readonly WorkShift _testShift;

    public ShiftControllerTest()
    {
        _mockService = new Mock<IShiftService>();
        _controller = new ShiftController(_mockService.Object);
        _testShift = TestShift;
    }

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

    [Fact]
    public async Task AmendWorkShift_WhenIdsDoNotMatch_ReturnsBadRequest()
    {
        var shiftId = Guid.NewGuid();

        var result = await _controller.AmendWorkShift(shiftId, _testShift);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.UpdateWorkShift(It.IsAny<WorkShift>()), Times.Never());
    }

    [Fact]
    public async Task AmendWorkShift_WhenServiceReturnsFalse_ReturnsNotFound()
    {
        _mockService.Setup(service => service.UpdateWorkShift(_testShift)).ReturnsAsync(false);
        
        var result = await _controller.AmendWorkShift(_testShift.ShiftId, _testShift);
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.NotNull(notFoundResult);
        
        _mockService.Verify(service => service.UpdateWorkShift(_testShift), Times.Once);
    }

    [Fact]
    public async Task AmendWorkShift_WhenServiceReturnsTrue_ReturnsOk()
    {
        _mockService.Setup(service => service.UpdateWorkShift(_testShift)).ReturnsAsync(true);
        
        var result = await _controller.AmendWorkShift(_testShift.ShiftId, _testShift);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        
        _mockService.Verify(service => service.UpdateWorkShift(_testShift), Times.Once());
    }

    [Fact]
    public async Task DeleteWorkShift_WhenShiftIdIsInvalid_ReturnsBadRequest()
    {
        const string invalidId = "guid";

        var result = await _controller.DeleteWorkShift(invalidId);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        
        _mockService.Verify(service => service.DeleteWorkShift(It.IsAny<Guid>()), Times.Never());
    }

    [Fact]
    public async Task DeleteWorkShift_WhenShiftDoesNotExist_ReturnsNotFound()
    {
        var shiftId = Guid.NewGuid();
        
        _mockService.Setup(service => service.DeleteWorkShift(shiftId)).ReturnsAsync(false);
        
        var result = await _controller.DeleteWorkShift(shiftId.ToString());
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.NotNull(notFoundResult.Value);
        
        _mockService.Verify(service => service.DeleteWorkShift(shiftId), Times.Once());
    }

    [Fact]
    public async Task DeleteWorkShift_WhenSuccessful_ReturnsOk()
    {
        var shiftId = Guid.NewGuid();
        
        _mockService.Setup(service => service.DeleteWorkShift(shiftId)).ReturnsAsync(true);
        
        var result = await _controller.DeleteWorkShift(shiftId.ToString());
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        
        _mockService.Verify(service => service.DeleteWorkShift(shiftId), Times.Once());
    }
}
