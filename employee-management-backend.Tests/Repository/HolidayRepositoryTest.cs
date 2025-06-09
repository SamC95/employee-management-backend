using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class HolidayRepositoryTest
{
    private readonly HolidayDbContext _context;
    private readonly HolidayRepository _repository;

    public HolidayRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<HolidayDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new HolidayDbContext(dbOptions);
        _repository = new HolidayRepository(_context);
    }

    private readonly HolidayEvent _holidayEvent = new()
    {
        EventId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        Status = "in-progress",
    };

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
}
