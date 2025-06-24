using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;

namespace employee_management_backend.Tests.Service;

public class PayslipServiceTest
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IPayslipRepository> _mockPayslipRepository;
    private readonly PayslipService _service;

    public PayslipServiceTest()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockPayslipRepository = new Mock<IPayslipRepository>();
        _service = new PayslipService(_mockPayslipRepository.Object, _mockEmployeeRepository.Object);
    }

    private readonly Payslip _testPayslip = new()
    {
        EmployeeId = "7734021",
        EmployeeName = "Hassan Patel",
        NationalInsuranceNumber = "QQ112233A",
        NationalInsuranceCategory = "A",
        PayslipStartDate = new DateOnly(2025, 7, 1),
        PayslipEndDate = new DateOnly(2025, 7, 31),
        CompanyName = "Vertex Innovations Ltd",
        EmployeeDepartment = "Product Development",
        DaysWorkedPerWeek = 5,
        TaxCode = "1257L",
        HoursWorked = 168,
        HolidayHours = 10,
        HasPension = true,
        HasUnion = false,
        PensionContributionPercentage = 4,
        UnionContributionPercentage = 0,
        PayPerHour = 23.40m,
        SickDates = [new DateOnly(2025, 7, 11)]
    };

    private readonly Employee _testEmployee = new()
    {
        EmployeeId = "7734021",
        ClockId = "9988776",
        FirstName = "Hassan",
        LastName = "Patel",
        Email = "hassan.patel@vertexinnovations.co.uk",
        PhoneNum = "07700 900234",
        Address = "42 Tech Park Avenue",
        City = "Manchester",
        PostCode = "M1 4AB",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1990,
            3,
            14),
        DateHired = new DateOnly(2022,
            6,
            1),
        IsAdmin = false,
        IsManager = true,
        IsActive = true,
        JobTitle = "Product Developer",
        NationalInsuranceNumber = "BB1234567",
        NationalInsuranceCategory = "A",
        TaxCode = "1257L",
        HasPension = false,
        EmployeePensionContributionPercentage = 0,
        EmployerPensionContributionPercentage = 0,
        HasUnion = false,
        UnionContributionPercentage = 0
    };

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
        _testPayslip.DaysWorkedPerWeek = 0;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Days worked per week must be greater than 0", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenHoursWorkedOrHolidayHoursAreNegative_ThrowsArgumentException()
    {
        _testPayslip.HoursWorked = -5;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Hours worked or holiday hours cannot be a negative number", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenStartDateIsAfterOrEqualToEndDate_ThrowsArgumentException()
    {
        _testPayslip.PayslipStartDate = new DateOnly(2025, 6, 30);
        _testPayslip.PayslipEndDate = new DateOnly(2025, 6, 1);
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Payslip start date cannot be after or equal to the payslip end date", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenPensionContributionIsPositiveButHasPensionFalse_ThrowsArgumentException()
    {
        _testPayslip.HasPension = false;
        
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePayslip(_testPayslip));
        Assert.Equal("Pension contribution percentage must be 0 if employee does not have a pension", exception.Message);
    }

    [Fact]
    public async Task CreatePayslip_WhenUnionContributionIsPositiveButHasUnionFalse_ThrowsArgumentException()
    {
        _testPayslip.HasUnion = false;
        _testPayslip.UnionContributionPercentage = 1.5m;
        
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
