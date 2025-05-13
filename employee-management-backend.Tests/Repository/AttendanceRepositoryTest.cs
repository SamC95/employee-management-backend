using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class AttendanceRepositoryTest
{
    private readonly AttendanceDbContext _context;
    private readonly AttendanceRepository _repository;


    public AttendanceRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<AttendanceDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ensures isolation between tests
            .Options;

        _context = new AttendanceDbContext(dbOptions);
        _repository = new AttendanceRepository(_context);
    }

    [Fact]
    public async Task SaveClockEvent_SavesSuccessfully()
    {
        var clockEvent = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-in",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid(),
        };

        await _repository.SaveClockEvent(clockEvent);
        
        var savedEvent =
            await _context.attendance.FirstOrDefaultAsync(storedEvent =>
                storedEvent.EventId == clockEvent.EventId);
        
        Assert.NotNull(savedEvent);
        Assert.Equal(clockEvent.ClockId, savedEvent.ClockId);
        Assert.Equal(clockEvent.Type, savedEvent.Type);
    }

    [Fact]
    public async Task GetClockEventsByClockId_ReturnsCorrectEvents()
    {
        var clockEvent1 = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-in",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };
        var clockEvent2 = new ClockEvent
        {
            ClockId = "1234567",
            Type = "clock-out",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };
        var clockEvent3 = new ClockEvent
        {
            ClockId = "9999999",
            Type = "clock-out",
            Timestamp = DateTime.Now,
            EventId = Guid.NewGuid()
        };
        
        _context.attendance.AddRange(clockEvent1, clockEvent2, clockEvent3);
        await _context.SaveChangesAsync();
        
        var result = await _repository.GetClockEventsByClockId("1234567");
        
        Assert.Equal(2, result.Count);
        Assert.All(result, clockEvent => Assert.Equal("1234567", clockEvent.ClockId));
    }

    [Fact]
    public async Task GetClockEventsByEventId_NoMatch_ReturnsEmptyList()
    {
        var result = await _repository.GetClockEventsByClockId("9999999");
        
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
