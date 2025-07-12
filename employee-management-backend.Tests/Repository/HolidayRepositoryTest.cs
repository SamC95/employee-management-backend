using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using static employee_management_backend.Tests.MockObjects.MockHolidayEvents;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class HolidayRepositoryTest
{
    private readonly HolidayDbContext _context;
    private readonly HolidayRepository _repository;
    private readonly HolidayEvent _holidayEvent;

    public HolidayRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<HolidayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HolidayDbContext(dbOptions);
        _repository = new HolidayRepository(_context);

        _holidayEvent = InProgressHolidayEvent;
    }

    [Fact]
    public async Task CreateHolidayRequest_ShouldSuccessfullySaveHolidayRequest()
    {
        await _repository.CreateHolidayRequest(_holidayEvent);

        var savedShift =
            await _context.Holidays.FirstOrDefaultAsync(storedShift => storedShift.EventId == _holidayEvent.EventId);
        
        Assert.NotNull(savedShift);
        Assert.Equal(_holidayEvent.EventId, savedShift.EventId);
        Assert.Equal(_holidayEvent.Date, savedShift.Date);
        Assert.Equal(_holidayEvent.Status, savedShift.Status);
        Assert.Equal(_holidayEvent.EmployeeId, savedShift.EmployeeId);
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenHolidayExists_UpdatesAndReturnsTrue()
    {
        _context.Holidays.Add(_holidayEvent);
        await _context.SaveChangesAsync();
        
        var result = await _repository.UpdateHolidayStatus(_holidayEvent);
        
        Assert.True(result);
        
        var updated = await _context.Holidays.FindAsync(_holidayEvent.EventId);
        Assert.Equal(_holidayEvent.Status, updated?.Status);
    }

    [Fact]
    public async Task UpdateHolidayStatus_WhenHolidayDoesNotExist_ThrowsException()
    {
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.UpdateHolidayStatus(_holidayEvent));
        
        Assert.Contains($"Holiday event with id {_holidayEvent.EventId} does not exist", exception.Message);
    }
}
