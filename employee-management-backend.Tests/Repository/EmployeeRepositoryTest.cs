using employee_management_backend.Database;
using employee_management_backend.Model;
using employee_management_backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class EmployeeRepositoryTest
{
    private readonly EmployeeDbContext _context;
    private readonly EmployeeRepository _repository;

    public EmployeeRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<EmployeeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new EmployeeDbContext(dbOptions);
        _repository = new EmployeeRepository(_context);
    }

    private readonly Employee _testEmployee = new()
    {
        EmployeeId = "12345678",
        ClockId = "5843292",
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@gmail.com",
        PhoneNum = "555-555-5555",
        Address = "Address 1",
        City = "City",
        PostCode = "PostCode",
        Country = "United Kingdom",
        Gender = "Male",
        DateOfBirth = new DateOnly(1991, 12, 31),
        DateHired = new DateOnly(2021, 05, 12),
        IsAdmin = false,
        IsManager = false,
        IsActive = true,
    };

    [Fact]
    public async Task CreateEmployee_AddsEmployeeAndSavesSuccessfully()
    {
        await _repository.CreateEmployee(_testEmployee);

        var savedEmployee =
            await _context.Employees.FirstOrDefaultAsync(storedEmployee =>
                storedEmployee.EmployeeId == _testEmployee.EmployeeId);

        Assert.NotNull(savedEmployee);
        Assert.Equal(_testEmployee.EmployeeId, savedEmployee.EmployeeId);
        Assert.Equal(_testEmployee.FirstName, savedEmployee.FirstName);
        Assert.Equal(_testEmployee.LastName, savedEmployee.LastName);
        Assert.Equal(_testEmployee.Email, savedEmployee.Email);
        Assert.Equal(_testEmployee.PhoneNum, savedEmployee.PhoneNum);
        Assert.Equal(_testEmployee.Address, savedEmployee.Address);
        Assert.Equal(_testEmployee.City, savedEmployee.City);
        Assert.Equal(_testEmployee.PostCode, savedEmployee.PostCode);
        Assert.Equal(_testEmployee.Country, savedEmployee.Country);
        Assert.Equal(_testEmployee.Gender, savedEmployee.Gender);
        Assert.Equal(_testEmployee.DateOfBirth, savedEmployee.DateOfBirth);
        Assert.Equal(_testEmployee.DateHired, savedEmployee.DateHired);
        Assert.Equal(_testEmployee.IsAdmin, savedEmployee.IsAdmin);
        Assert.Equal(_testEmployee.IsManager, savedEmployee.IsManager);
        Assert.Equal(_testEmployee.IsActive, savedEmployee.IsActive);
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsEmployee_WhenFound()
    {
        _context.Employees.Add(_testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _repository.GetEmployeeById(_testEmployee.EmployeeId);
        
        Assert.NotNull(result);
        
        Assert.Equal(_testEmployee.EmployeeId, result.EmployeeId);
        Assert.Equal(_testEmployee.FirstName, result.FirstName);
        Assert.Equal(_testEmployee.LastName, result.LastName);
        Assert.Equal(_testEmployee.Email, result.Email);
        Assert.Equal(_testEmployee.PhoneNum, result.PhoneNum);
        Assert.Equal(_testEmployee.Address, result.Address);
        Assert.Equal(_testEmployee.City, result.City);
        Assert.Equal(_testEmployee.PostCode, result.PostCode);
        Assert.Equal(_testEmployee.Country, result.Country);
        Assert.Equal(_testEmployee.Gender, result.Gender);
        Assert.Equal(_testEmployee.DateOfBirth, result.DateOfBirth);
        Assert.Equal(_testEmployee.DateHired, result.DateHired);
        Assert.Equal(_testEmployee.IsAdmin, result.IsAdmin);
        Assert.Equal(_testEmployee.IsManager, result.IsManager);
        Assert.Equal(_testEmployee.IsActive, result.IsActive);
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsNull_WhenNotFound()
    {
        const string employeeId = "9999999";
        
        var result = await _repository.GetEmployeeById(employeeId);
        
        Assert.Null(result);
    }

    [Fact]
    public async Task CheckClockIdExists_ReturnsTrue_WhenFound()
    {
        _context.Employees.Add(_testEmployee);
        await _context.SaveChangesAsync();
        
        var result = await _repository.CheckClockIdExists(_testEmployee.ClockId);
        
        Assert.NotNull(result);
        Assert.True(result);
    }

    [Fact]
    public async Task CheckClockIdExists_ReturnsFalse_WhenNotFound()
    {
        const string clockId = "999999";
        
        var result = await _repository.CheckClockIdExists(clockId);
        
        Assert.NotNull(result);
        Assert.False(result);
    }
}
