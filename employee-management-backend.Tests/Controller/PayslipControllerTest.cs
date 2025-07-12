using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using static employee_management_backend.Tests.MockObjects.MockPayslips;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class PayslipControllerTest
{
    private readonly Mock<IPayslipService> _mockService;
    private readonly PayslipController _payslipController;
    private readonly Payslip _testPayslip;

    public PayslipControllerTest()
    {
        _mockService = new Mock<IPayslipService>();
        _payslipController = new PayslipController(_mockService.Object);
        _testPayslip = CreatePayslip();
    }

    [Fact]
    public async Task CreatePayslip_WhenSuccessful_ReturnsOk()
    {
        var result = await _payslipController.CreatePayslip(_testPayslip);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Payslip successfully created", okResult.Value.ToString());

        _mockService.Verify(service => service.CreatePayslip(_testPayslip), Times.Once());
    }

    [Fact]
    public async Task CreatePayslip_WhenArgumentExceptionThrown_ReturnsBadRequest()
    {
        _mockService.Setup(service => service.CreatePayslip(It.IsAny<Payslip>()))
            .Throws(new ArgumentException("Employee not found"));

        var result = await _payslipController.CreatePayslip(_testPayslip);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.NotNull(badRequestResult.Value);
        Assert.Contains("Employee not found", badRequestResult.Value.ToString());

        _mockService.Verify(service => service.CreatePayslip(_testPayslip), Times.Once());
    }

    [Fact]
    public async Task CreatePayslip_WhenUnhandledExceptionThrown_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.CreatePayslip(_testPayslip))
            .ThrowsAsync(new Exception("Database failure"));
        
        var result = await _payslipController.CreatePayslip(_testPayslip);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        Assert.Contains("Database failure", objectResult.Value.ToString());
        
        _mockService.Verify(service => service.CreatePayslip(_testPayslip), Times.Once());
    }
}
