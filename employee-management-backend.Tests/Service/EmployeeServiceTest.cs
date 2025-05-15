using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;

namespace employee_management_backend.Tests.Service;

public class EmployeeServiceTest
{
    private readonly Mock<IEmployeeRepository> _mockRepository;
    private readonly EmployeeService _employeeService;

    public EmployeeServiceTest()
    {
        _mockRepository = new Mock<IEmployeeRepository>();
        _employeeService = new EmployeeService(_mockRepository.Object);
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
        JobTitle = "Graphic Designer",
    };

    [Fact]
    public async Task CreateEmployee_CallsRepository()
    {
        await _employeeService.CreateEmployee(_testEmployee);

        _mockRepository.Verify(repo =>
            repo.CreateEmployee(It.Is<Employee>(employee => employee.EmployeeId == _testEmployee.EmployeeId)));
    }

    [Fact]
    public async Task UpdateEmployeeDetails_ExistingEmployee_UpdatesFieldsAndReturnsTrue()
    {
        var existingEmployee = _testEmployee;
        var patch = new EmployeeUpdater { EmployeeId = existingEmployee.EmployeeId, FirstName = "Jane" };

        _mockRepository.Setup(request => request.GetEmployeeById(existingEmployee.EmployeeId))
            .ReturnsAsync(existingEmployee);
        _mockRepository.Setup(request => request.UpdateEmployeeDetails(It.IsAny<Employee>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var result = await _employeeService.UpdateEmployeeDetails(patch);

        Assert.True(result);
        Assert.Equal("Jane", existingEmployee.FirstName);
        _mockRepository.Verify(request => request.UpdateEmployeeDetails(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeDetails_EmployeeNotFound_ReturnsFalse()
    {
        var patch = new EmployeeUpdater { EmployeeId = "9999999", FirstName = "Bobby" };
        _mockRepository.Setup(request => request.GetEmployeeById("9999999"))
            .ReturnsAsync((Employee?)null);
        
        var result = await _employeeService.UpdateEmployeeDetails(patch);
        
        Assert.False(result);
        _mockRepository.Verify(request => request.UpdateEmployeeDetails(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task GetEmployeeById_CallsRepository()
    {
        await _employeeService.GetEmployeeById(_testEmployee.EmployeeId);

        _mockRepository.Verify(repo => repo.GetEmployeeById(_testEmployee.EmployeeId));
    }

    [Fact]
    public async Task GetEmployeesByJobTitle_CapitalisesTitleAndReturnsResults()
    {
        const string inputTitle = "graphic designer";
        const string expectedTitle = "Graphic Designer";

        var expectedEmployees = new List<Employee>
        {
            _testEmployee,
        };

        _mockRepository.Setup(repo => repo.GetEmployeesByJobTitle(expectedTitle)).ReturnsAsync(expectedEmployees);

        var result = await _employeeService.GetEmployeesByJobTitle(inputTitle);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(expectedTitle, result.First().JobTitle);

        _mockRepository.Verify(repo => repo.GetEmployeesByJobTitle(expectedTitle), Times.Once);
    }

    [Fact]
    public async Task GetEmployeesByJobTitle_WithCapitalisedInput_CallsRepositoryCorrectly()
    {
        const string inputTitle = "Software Developer";

        var expectedEmployees = new List<Employee>();

        _mockRepository.Setup(repo => repo.GetEmployeesByJobTitle(inputTitle)).ReturnsAsync(expectedEmployees);

        var result = await _employeeService.GetEmployeesByJobTitle(inputTitle);

        Assert.Empty(result);

        _mockRepository.Verify(repo => repo.GetEmployeesByJobTitle(inputTitle), Times.Once);
    }

    [Fact]
    public async Task CheckClockIdExists_CallsRepository()
    {
        await _employeeService.CheckClockIdExists(_testEmployee.ClockId);

        _mockRepository.Verify(repo => repo.CheckClockIdExists(_testEmployee.ClockId));
    }
}
