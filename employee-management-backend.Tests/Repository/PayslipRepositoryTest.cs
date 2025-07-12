using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using static employee_management_backend.Tests.MockObjects.MockPayslips;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class PayslipRepositoryTest
{
    private readonly PayslipDbContext _context;
    private readonly PayslipRepository _repository;
    private readonly Payslip _testPayslip;

    public PayslipRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<PayslipDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PayslipDbContext(dbOptions);
        _repository = new PayslipRepository(_context);

        _testPayslip = CreateCalculatedPayslip();
    }

    [Fact]
    public async Task CreatePayslip_ShouldSuccessfullySavePayslip()
    {
        await _repository.CreatePayslip(_testPayslip);

        var savedPayslip =
            await _context.Payslips.FirstOrDefaultAsync(storedPayslip =>
                storedPayslip.PayslipId == _testPayslip.PayslipId);
        
        Assert.NotNull(savedPayslip);
        Assert.Equal(_testPayslip.PayslipId, savedPayslip.PayslipId);
        Assert.Equal(_testPayslip.EmployeeId, savedPayslip.EmployeeId);
        Assert.Equal(_testPayslip.EmployeeName, savedPayslip.EmployeeName);
        Assert.Equal(_testPayslip.PayslipStartDate, savedPayslip.PayslipStartDate);
        Assert.Equal(_testPayslip.PayslipEndDate, savedPayslip.PayslipEndDate);
        Assert.Equal(_testPayslip.CompanyName, savedPayslip.CompanyName);
        Assert.Equal(_testPayslip.DaysWorkedPerWeek, savedPayslip.DaysWorkedPerWeek);
        Assert.Equal(_testPayslip.HoursWorked, savedPayslip.HoursWorked);
        Assert.Equal(_testPayslip.SickDates, savedPayslip.SickDates);
    }
}
