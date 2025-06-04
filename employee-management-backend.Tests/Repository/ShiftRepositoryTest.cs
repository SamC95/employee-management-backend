using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class ShiftRepositoryTest
{
    private readonly ShiftDbContext _context;
    private readonly ShiftRepository _repository;

    public ShiftRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<ShiftDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ShiftDbContext(dbOptions);
        _repository = new ShiftRepository(_context);
    }

    private readonly WorkShift _testShift = new()
    {
        ShiftId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        StartTime = new TimeOnly(08, 30, 00),
        EndTime = new TimeOnly(17, 30, 00)
    };

    [Fact]
    public async Task AddWorkShift_Should_SuccessfullySaveShift()
    {
        await _repository.AddWorkShift(_testShift);

        var savedShift =
            await _context.Shifts.FirstOrDefaultAsync(storedShift => storedShift.ShiftId == _testShift.ShiftId);
        
        Assert.NotNull(savedShift);
        Assert.Equal(_testShift.ShiftId, savedShift.ShiftId);
        Assert.Equal(_testShift.EmployeeId, savedShift.EmployeeId);
        Assert.Equal(_testShift.Date, savedShift.Date);
        Assert.Equal(_testShift.StartTime, savedShift.StartTime);
        Assert.Equal(_testShift.EndTime, savedShift.EndTime);
    }

    [Fact]
    public async Task UpdateWorkShift_WhenShiftExists_UpdatesAndReturnsTrue()
    {
        _context.Shifts.Add(_testShift);
        await _context.SaveChangesAsync();
        
        var result = await _repository.UpdateWorkShift(_testShift);
        
        Assert.True(result);
        
        var updated = await _context.Shifts.FindAsync(_testShift.ShiftId);
        Assert.Equal(new DateOnly(2018, 5, 5), updated?.Date);
        Assert.Equal(_testShift.StartTime, updated?.StartTime);
    }

    [Fact]
    public async Task UpdateWorkShift_WhenShiftDoesNotExist_ThrowsException()
    {
        var exception = await Assert.ThrowsAsync<Exception>(async () => await _repository.UpdateWorkShift(_testShift));
        
        Assert.Contains("was not found", exception.Message);
    }

    [Fact]
    public async Task DeleteWorkShift_WhenShiftExists_RemovesShiftAndReturnsTrue()
    {
        _context.Shifts.Add(_testShift);
        await _context.SaveChangesAsync();
        
        var shiftId = _testShift.ShiftId;
        
        var result = await _repository.DeleteWorkShift(shiftId);
        
        Assert.True(result);
        var deletedShift = await _context.Shifts.FindAsync(shiftId);
        
        Assert.Null(deletedShift);
    }

    [Fact]
    public async Task DeleteWorkShift_WhenShiftDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.DeleteWorkShift(Guid.NewGuid());
        
        Assert.False(result);
    }
}
