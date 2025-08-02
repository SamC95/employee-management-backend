using System.Text.Json;
using employee_management_backend.Controller;
using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace employee_management_backend.Tests.Controller;

public class EmployeeControllerTest
{
    private readonly Mock<IEmployeeService> _mockService;
    private readonly EmployeeController _employeeController;
    private readonly Employee _testEmployee;

    public EmployeeControllerTest()
    {
        _mockService = new Mock<IEmployeeService>();
        _employeeController = new EmployeeController(_mockService.Object);
        _testEmployee = TestEmployeeJohn;
    }
    
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
    public async Task CreateEmployee_ValidEmployee_ReturnsTempPassword()
    {
        const string expectedPassword = "Temp123";
        _mockService.Setup(service => service.CreateEmployee(_testEmployee)).ReturnsAsync(expectedPassword);
        
        var result = await _employeeController.CreateEmployee(_testEmployee);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(okResult.Value);
        Assert.Contains("Employee created successfully", okResult.Value.ToString());
        
        var json = JsonSerializer.Serialize(okResult.Value);
        var response = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
        
        Assert.Equal("Employee created successfully", response["message"]);
        Assert.Equal(expectedPassword, response["temporaryPassword"]);

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
    public async Task GetEmployeesByJobTitle_ReturnsOk_WhenMatchFound()
    {
        const string jobTitle = "Graphic Designer";
        var employeeList = new List<Employee>
        {
            _testEmployee
        };
        
        _mockService.Setup(service => service.GetEmployeesByJobTitle(jobTitle)).ReturnsAsync(employeeList);

        var result = await _employeeController.GetEmployeesByJobTitle(jobTitle);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
        Assert.Single(returnValue);
        
        _mockService.Verify(service => service.GetEmployeesByJobTitle(jobTitle), Times.Once);
    }

    [Fact]
    public async Task GetEmployeesByJobTitle_ReturnsOk_WithEmptyList_WhenNoMatchFound()
    {
        const string jobTitle = "Software Developer";
        var emptyList = new List<Employee>();
        
        _mockService.Setup(service => service.GetEmployeesByJobTitle(jobTitle)).ReturnsAsync(emptyList);
        
        var result = await _employeeController.GetEmployeesByJobTitle(jobTitle);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
        Assert.Empty(returnValue);
        
        _mockService.Verify(service => service.GetEmployeesByJobTitle(jobTitle), Times.Once);
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
