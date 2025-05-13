using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service;
using Moq;

namespace employee_management_backend.Tests.Service;

public class AttendanceServiceTest
{
    private readonly Mock<IAttendanceRepository> _mockRepository;
    private readonly AttendanceService _attendanceService;

    public AttendanceServiceTest()
    {
        _mockRepository = new Mock<IAttendanceRepository>();
        _attendanceService = new AttendanceService(_mockRepository.Object);
    }

    [Fact]
    public async Task PostClockEvent_ConvertsTimestampToUtc_AndCallsRepository()
    {
        var localTime = new DateTime(2025, 1, 1, 1, 1, 1, DateTimeKind.Utc);
        var clockEvent = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-in",
            Timestamp = localTime
        };

        _mockRepository.Setup(repository => repository.SaveClockEvent(It.IsAny<ClockEvent>()))
            .Returns(Task.CompletedTask);

        await _attendanceService.PostClockEvent(clockEvent);

        _mockRepository.Verify(repository => repository.SaveClockEvent(It.Is<ClockEvent>(eventData =>
            eventData.Timestamp.Kind == DateTimeKind.Utc &&
            eventData.ClockId == "1234567" &&
            eventData.Type == "clock-in"
        )), Times.Once);
    }

    [Fact]
    public async Task GetClockEventsByClockId_CallsRepositoryAndReturnsResult()
    {
        const string clockId = "1234567";
        var eventData = new List<ClockEvent>
        {
            new() { ClockId = clockId, Type = "clock-in", Timestamp = DateTime.Now }
        };

        _mockRepository.Setup(repository => repository.GetClockEventsByClockId(clockId)).ReturnsAsync(eventData);

        var result = await _attendanceService.GetClockEventsByClockId(clockId);

        Assert.Equal(eventData, result);
        _mockRepository.Verify(repository => repository.GetClockEventsByClockId(clockId), Times.Once);
    }

    [Fact]
    public async Task GetClockEventsByClockId_NoEvents_ReturnsEmptyList()
    {
        const string clockId = "9999999";

        _mockRepository.Setup(repository => repository.GetClockEventsByClockId(clockId))
            .ReturnsAsync(new List<ClockEvent>());
        
        var result = await _attendanceService.GetClockEventsByClockId(clockId);
        
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockRepository.Verify(repository => repository.GetClockEventsByClockId(clockId), Times.Once);
    }
}
