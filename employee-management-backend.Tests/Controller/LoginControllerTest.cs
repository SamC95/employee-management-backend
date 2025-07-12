using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using static employee_management_backend.Tests.MockObjects.MockLogins;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class LoginControllerTest
{
    private readonly Mock<ILoginService> _mockService;
    private readonly LoginController _controller;
    private readonly LoginDetails _testLogin;

    public LoginControllerTest()
    {
        _mockService = new Mock<ILoginService>();
        _controller = new LoginController(_mockService.Object);
        _testLogin = TestLogin;
    }

    [Fact]
    public async Task ValidateLogin_WhenSuccessful_ReturnsOk()
    {
        var result = await _controller.ValidateLogin(_testLogin);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        
        _mockService.Verify(service => service.ValidateLogin(_testLogin), Times.Once);
    }

    [Fact]
    public async Task ValidateLogin_WhenUnhandledExceptionThrown_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.ValidateLogin(_testLogin)).Throws(new Exception());
        
        var result = await _controller.ValidateLogin(_testLogin);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.ValidateLogin(_testLogin), Times.Once);
    }
}
