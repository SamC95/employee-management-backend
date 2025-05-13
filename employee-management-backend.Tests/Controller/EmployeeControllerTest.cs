using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class EmployeeControllerTest
{
    private readonly Mock<IEmployeeService> _mockService;
    private readonly EmployeeController _employeeController;

    public EmployeeControllerTest()
    {
        _mockService = new Mock<IEmployeeService>();
        _employeeController = new EmployeeController(_mockService.Object);
    }

    private readonly Employee _testEmployee = new()
    {
        EmployeeId = "12345678",
        ClockId = "5843292",
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@gmail.com",
        PhoneNum = "555-555-5555",
        Address = "Address 1",
        City = "City",
        PostCode = "PostCode",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1991, 12, 31),
        DateHired = new DateOnly(2021, 05, 12),
        IsAdmin = false,
        IsManager = false,
        IsActive = true,
    };

    /*
    TODO: Employee database fields are still subject to change based on needs.
    TODO: Any employee unit tests may need refactoring when any changes are made
    */

    [Fact]
    public async Task CreateEmployee_ValidEmployee_ReturnsOk()
    {
        var result = await _employeeController.CreateEmployee(_testEmployee);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Employee created successfully", okResult.Value.ToString());

        _mockService.Verify(service => service.CreateEmployee(_testEmployee), Times.Once);
    }

    [Fact]
    public async Task CreateEmployee_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.CreateEmployee(It.IsAny<Employee>())).Throws<Exception>();
        
        var result = await _employeeController.CreateEmployee(_testEmployee);
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.CreateEmployee(_testEmployee), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeById_ValidId_ReturnsOk()
    {
        _mockService.Setup(service => service.GetEmployeeById("12345678")).ReturnsAsync(_testEmployee);
        
        var result = await _employeeController.GetEmployeeById("12345678");
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Equal(_testEmployee, okResult.Value);
        
        _mockService.Verify(service => service.GetEmployeeById("12345678"), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeById_NoMatch_ReturnsEmptyObject()
    {
        const string employeeId = "9999999";
        
        _mockService.Setup(service => service.GetEmployeeById(employeeId)).ReturnsAsync((Employee?)null);
        
        var result = await _employeeController.GetEmployeeById(employeeId);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var returnedValue = okResult.Value;
        Assert.NotNull(returnedValue);
        
        var returnedAnonymousType = returnedValue.GetType();

        var properties = returnedAnonymousType.GetProperties();
        Assert.Empty(properties);
    }

    [Fact]
    public async Task GetEmployeeById_WhenServiceFails_ReturnsInternalServerError()
    {
        _mockService.Setup(service => service.GetEmployeeById(It.IsAny<string>())).Throws<Exception>();
        
        var result = await _employeeController.GetEmployeeById("12345678");
        
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.NotNull(objectResult.Value);
        
        _mockService.Verify(service => service.GetEmployeeById("12345678"), Times.Once);
    }

    [Fact]
    public async Task CheckClockIdExists_ReturnsTrue_WhenClockIdExists()
    {
        const string clockId = "1234567";
        
        _mockService.Setup(service => service.CheckClockIdExists(clockId)).ReturnsAsync(true);
        
        var result = await _employeeController.CheckClockIdExists(clockId);
        
        var objectResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.NotNull(objectResult.Value);
        Assert.Equal(200, objectResult.StatusCode);
        Assert.Equal(true, objectResult.Value);
        
        _mockService.Verify(service => service.CheckClockIdExists(clockId), Times.Once);
    }

    [Fact]
    public async Task CheckClockIdExists_ReturnsFalse_WhenClockIdDoesNotExist()
    {
        const string clockId = "999999";
        
        _mockService.Setup(service => service.CheckClockIdExists(clockId)).ReturnsAsync(false);
        
        var result = await _employeeController.CheckClockIdExists(clockId);
        
        var objectResult = Assert.IsType<OkObjectResult>(result);
        
        Assert.NotNull(objectResult.Value);
        Assert.Equal(200, objectResult.StatusCode);
        Assert.Equal(false, objectResult.Value);
        
        _mockService.Verify(service => service.CheckClockIdExists(clockId), Times.Once);
    }

    [Fact]
    public async Task CheckClockIdExists_WhenServiceFails_ReturnsInternalServerError()
    {
        const string clockId = "1234567";
        _mockService.Setup(service => service.CheckClockIdExists(clockId)).Throws<Exception>();
        
        var result = await _employeeController.CheckClockIdExists(clockId);
        var objectResult = Assert.IsType<ObjectResult>(result);
        
        Assert.NotNull(objectResult.Value);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("unexpected error", objectResult.Value.ToString());
        
        _mockService.Verify(service => service.CheckClockIdExists(clockId), Times.Once);
    }
}
