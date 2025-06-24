using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;

namespace employee_management_backend.Tests.Service;

public class ShiftServiceTest
{
    private readonly Mock<IShiftRepository> _mockShiftRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly ShiftService _service;

    public ShiftServiceTest()
    {
        _mockShiftRepository = new Mock<IShiftRepository>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new ShiftService(_mockShiftRepository.Object, _mockEmployeeRepository.Object);
    }
    
    private readonly WorkShift _testShift = new()
    {
        ShiftId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        StartTime = new TimeOnly(08, 30, 00),
        EndTime = new TimeOnly(17, 30, 00)
    };

    private static Employee CreateTestEmployee(string id)
    {
        return new Employee
        {
            EmployeeId = id,
            ClockId = "123456",
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNum = "0000000000",
            Address = "123 Street",
            City = "City",
            PostCode = "00000",
            Country = "Country",
            Gender = "Test",
            DateOfBirth = new DateOnly(1990,
                1,
                1),
            DateHired = new DateOnly(2020,
                1,
                1),
            IsAdmin = false,
            IsManager = false,
            IsActive = true,
            JobTitle = "Tester",
            NationalInsuranceNumber = "WW7654321",
            NationalInsuranceCategory = "A",
            TaxCode = "1257L",
            HasPension = false,
            EmployeePensionContributionPercentage = 0,
            EmployerPensionContributionPercentage = 0,
            HasUnion = false,
            UnionContributionPercentage = 0
        };
    }
    
    [Fact]
    public async Task AddWorkShift_AddsShift_WhenEmployeeExists()
    {
        if (_testShift.EmployeeId != null)
            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeeById(_testShift.EmployeeId))
                .ReturnsAsync(CreateTestEmployee(_testShift.EmployeeId));

        await _service.AddWorkShift(_testShift);
        
        _mockShiftRepository.Verify(repo => repo.AddWorkShift(_testShift), Times.Once);
    }

    [Fact]
    public async Task AddWorkShift_ThrowsArgumentException_WhenEmployeeDoesntExist()
    {
        _mockEmployeeRepository
            .Setup(repo => repo.GetEmployeeById(_testShift.EmployeeId))
            .ReturnsAsync((Employee?)null);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddWorkShift(_testShift));
        
        _mockShiftRepository.Verify(repo => repo.AddWorkShift(_testShift), Times.Never);
    }

    [Fact]
    public async Task UpdateWorkShift_WhenRepositoryReturnsTrue_ReturnsTrue()
    {
        _mockShiftRepository.Setup(repo => repo.UpdateWorkShift(_testShift)).ReturnsAsync(true);
        
        var result = await _service.UpdateWorkShift(_testShift);
        
        Assert.True(result);
        _mockShiftRepository.Verify(repo => repo.UpdateWorkShift(_testShift), Times.Once);
    }

    [Fact]
    public async Task UpdateWorkShift_WhenRepositoryReturnsFalse_ReturnsFalse()
    {
        _mockShiftRepository.Setup(repo => repo.UpdateWorkShift(_testShift)).ReturnsAsync(false);
        
        var result = await _service.UpdateWorkShift(_testShift);
        
        Assert.False(result);
        _mockShiftRepository.Verify(repo => repo.UpdateWorkShift(_testShift), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkShift_WhenRepositoryReturnsTrue_ReturnsTrue()
    {
        _mockShiftRepository.Setup(repo => repo.DeleteWorkShift(_testShift.ShiftId)).ReturnsAsync(true);
        
        var result = await _service.DeleteWorkShift(_testShift.ShiftId);
        
        Assert.True(result);
        _mockShiftRepository.Verify(repo => repo.DeleteWorkShift(_testShift.ShiftId), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkShift_WhenRepositoryReturnsFalse_ReturnsFalse()
    {
        _mockShiftRepository.Setup(repo => repo.DeleteWorkShift(_testShift.ShiftId)).ReturnsAsync(false);
        
        var result = await _service.DeleteWorkShift(_testShift.ShiftId);
        
        Assert.False(result);
        _mockShiftRepository.Verify(repo => repo.DeleteWorkShift(_testShift.ShiftId), Times.Once);
    }
}
