using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using static employee_management_backend.Tests.MockObjects.MockEmployees;
using static employee_management_backend.Tests.MockObjects.MockPayslips;
using Moq;

namespace employee_management_backend.Tests.Service;

public class PayslipServiceTest
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IPayslipRepository> _mockPayslipRepository;
    private readonly PayslipService _service;
    private readonly Payslip _testPayslip;
    private readonly Employee _testEmployee;

    public PayslipServiceTest()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockPayslipRepository = new Mock<IPayslipRepository>();
        _service = new PayslipService(_mockPayslipRepository.Object, _mockEmployeeRepository.Object);
        _testPayslip = CreatePayslip();
        _testEmployee = TestEmployeeHassan;
    }

    [Fact]
    public async Task CreatePayslip_WhenValidInput_CreatesPayslip()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        
        await _service.CreatePayslip(_testPayslip);
        
        _mockPayslipRepository.Verify(repo => repo.CreatePayslip(_testPayslip), Times.Once);
        _mockEmployeeRepository.Verify(repo => repo.GetEmployeeById(_testPayslip.EmployeeId), Times.Once);
    }

    [Fact]
    public async Task CreatePayslip_WhenNull_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreatePayslip(null));
    }

    [Fact]
    public async Task CreatePayslip_WhenDaysWorkedPerWeekIsZero_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        _testPayslip.DaysWorkedPerWeek = 0;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Days worked per week must be greater than 0", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenHoursWorkedOrHolidayHoursAreNegative_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        _testPayslip.HoursWorked = -5;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Hours worked or holiday hours cannot be a negative number", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenStartDateIsAfterOrEqualToEndDate_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        
        _testPayslip.PayslipStartDate = new DateOnly(2025, 6, 30);
        _testPayslip.PayslipEndDate = new DateOnly(2025, 6, 1);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Payslip start date cannot be after or equal to the payslip end date", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenEmployeePensionContributionIsPositiveButHasPensionFalse_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        _testEmployee.HasPension = false;
        _testEmployee.EmployeePensionContributionPercentage = 1.5m;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Employee pension contribution percentage must be 0 if employee does not have a pension", exception.Message);
    }
    
    [Fact]
    public async Task CreatePayslip_WhenEmployerPensionContributionIsPositiveButHasPensionFalse_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        _testEmployee.HasPension = false;
        _testEmployee.EmployerPensionContributionPercentage = 3.0m;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Employer pension contribution percentage must be 0 if employee does not have a pension", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenUnionContributionIsPositiveButHasUnionFalse_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId))
            .ReturnsAsync(_testEmployee);
        
        _testEmployee.HasUnion = false;
        _testEmployee.UnionContributionPercentage = 1.5m;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Union contribution percentage must be 0 if employee does not have an union", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenEmployeeNotFound_ThrowsArgumentException()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_testPayslip.EmployeeId)).ReturnsAsync((Employee)null);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Employee not found", exception.Message);
    }
}
