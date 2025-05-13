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
    };

    [Fact]
    public async Task CreateEmployee_CallsRepository()
    {
        await _employeeService.CreateEmployee(_testEmployee);

        _mockRepository.Verify(repo =>
            repo.CreateEmployee(It.Is<Employee>(employee => employee.EmployeeId == _testEmployee.EmployeeId)));
    }

    [Fact]
    public async Task GetEmployeeById_CallsRepository()
    {
        await _employeeService.GetEmployeeById(_testEmployee.EmployeeId);
        
        _mockRepository.Verify(repo => repo.GetEmployeeById(_testEmployee.EmployeeId));
    }
}
