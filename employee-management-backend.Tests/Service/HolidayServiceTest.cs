using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;
using static employee_management_backend.Tests.MockObjects.MockHolidayEvents;
using static employee_management_backend.Tests.MockObjects.MockEmployees;

namespace employee_management_backend.Tests.Service;

public class HolidayServiceTest
{
    private readonly Mock<IHolidayRepository> _mockHolidayRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly HolidayService _service;

    public HolidayServiceTest()
    {
        _mockHolidayRepository = new Mock<IHolidayRepository>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _service = new HolidayService(_mockHolidayRepository.Object, _mockEmployeeRepository.Object);
    }

    [Fact]
    public async Task CreateHolidayRequest_AddsRequest_WhenEmployeeExists()
    {
        if (InProgressHolidayEvent.EmployeeId != null)
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(InProgressHolidayEvent.EmployeeId))
                .ReturnsAsync(CreateTestEmployee(InProgressHolidayEvent.EmployeeId));

        await _service.CreateHolidayRequest(InProgressHolidayEvent);

        _mockHolidayRepository.Verify(repo => repo.CreateHolidayRequest(InProgressHolidayEvent), Times.Once);
    }

    [Fact]
    public async Task CreateHolidayRequest_ThrowsArgumentException_WhenEmployeeDoesNotExist()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(InProgressHolidayEvent.EmployeeId))
            .ReturnsAsync((Employee?)null);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateHolidayRequest(InProgressHolidayEvent));
        
        _mockHolidayRepository.Verify(repo => repo.CreateHolidayRequest(InProgressHolidayEvent), Times.Never);
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenRepositoryReturnsFalse_ReturnsFalse()
    {
        _mockHolidayRepository.Setup(repo => repo.UpdateHolidayStatus(InProgressHolidayEvent)).ReturnsAsync(false);
        
        var result = await _service.UpdateHolidayStatus(InProgressHolidayEvent);
        
        Assert.False(result);
        _mockHolidayRepository.Verify(repo => repo.UpdateHolidayStatus(InProgressHolidayEvent), Times.Once);
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenRepositoryReturnsTrue_ReturnsTrue()
    {
        _mockHolidayRepository.Setup(repo => repo.UpdateHolidayStatus(InProgressHolidayEvent)).ReturnsAsync(true);
        
        var result = await _service.UpdateHolidayStatus(InProgressHolidayEvent);
        
        Assert.True(result);
        _mockHolidayRepository.Verify(repo => repo.UpdateHolidayStatus(InProgressHolidayEvent), Times.Once);
    }
}
