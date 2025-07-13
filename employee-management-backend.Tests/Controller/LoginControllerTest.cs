using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Authentication.Interface;
using static employee_management_backend.Tests.MockObjects.MockLogins;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class LoginControllerTest
{
    private readonly Mock<ILoginService> _mockService;
    private readonly Mock<IJwtService> _mockJwtService;
    private readonly LoginController _controller;
    private readonly LoginDetails _testLogin;
    private readonly Employee _testEmployee;

    public LoginControllerTest()
    {
        _mockService = new Mock<ILoginService>();
        _mockJwtService = new Mock<IJwtService>();
        _controller = new LoginController(_mockService.Object, _mockJwtService.Object);
        _testLogin = TestLogin;
        _testEmployee = TestEmployeeHassan;
    }

    [Fact]
    public async Task ValidateLogin_WhenSuccessful_ReturnsOk()
    {
        _mockService.Setup(service => service.ValidateLogin(_testLogin))
            .ReturnsAsync(_testEmployee);

        _mockJwtService.Setup(jwt =>
                jwt.GenerateJwtToken(_testEmployee.EmployeeId, $"{_testEmployee.FirstName} {_testEmployee.LastName}"))
            .Returns("mocked-jwt-token");

        var result = await _controller.ValidateLogin(_testLogin);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var value = okResult.Value;

        var token = value?.GetType().GetProperty("token");
        Assert.NotNull(token);
       
        Assert.Equal("mocked-jwt-token", token.GetValue(value) as string);

        _mockService.Verify(service => service.ValidateLogin(_testLogin), Times.Once);
        _mockJwtService.Verify(
            jwt => jwt.GenerateJwtToken(_testEmployee.EmployeeId,
                $"{_testEmployee.FirstName} {_testEmployee.LastName}"), Times.Once);
    }

    [Fact]
    public async Task ValidateLogin_WhenUnhandledExceptionThrown_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.ValidateLogin(_testLogin)).Throws(new Exception());

        var result = await _controller.ValidateLogin(_testLogin);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);

        _mockService.Verify(service => service.ValidateLogin(_testLogin), Times.Once);
        _mockJwtService.VerifyNoOtherCalls();
    }
}