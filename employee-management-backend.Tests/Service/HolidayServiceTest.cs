using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;

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

    private readonly HolidayEvent _holidayEvent = new()
    {
        EventId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        Status = "in-progress",
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
            DateOfBirth = new DateOnly(1990, 1, 1),
            DateHired = new DateOnly(2020, 1, 1),
            IsAdmin = false,
            IsManager = false,
            IsActive = true,
            JobTitle = "Tester"
        };
    }

    [Fact]
    public async Task CreateHolidayRequest_AddsRequest_WhenEmployeeExists()
    {
        if (_holidayEvent.EmployeeId != null)
            _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_holidayEvent.EmployeeId))
                .ReturnsAsync(CreateTestEmployee(_holidayEvent.EmployeeId));

        await _service.CreateHolidayRequest(_holidayEvent);

        _mockHolidayRepository.Verify(repo => repo.CreateHolidayRequest(_holidayEvent), Times.Once);
    }

    [Fact]
    public async Task CreateHolidayRequest_ThrowsArgumentException_WhenEmployeeDoesNotExist()
    {
        _mockEmployeeRepository.Setup(repo => repo.GetEmployeeById(_holidayEvent.EmployeeId))
            .ReturnsAsync((Employee?)null);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateHolidayRequest(_holidayEvent));
        
        _mockHolidayRepository.Verify(repo => repo.CreateHolidayRequest(_holidayEvent), Times.Never);
    }
}
