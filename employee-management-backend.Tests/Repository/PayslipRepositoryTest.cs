using employee_management_backend.Model;
using employee_management_backend.Repository;
using employee_management_backend.Repository.Database;
using Microsoft.EntityFrameworkCore;

namespace employee_management_backend.Tests.Repository;

public class PayslipRepositoryTest
{
    private readonly PayslipDbContext _context;
    private readonly PayslipRepository _repository;

    public PayslipRepositoryTest()
    {
        var dbOptions = new DbContextOptionsBuilder<PayslipDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new PayslipDbContext(dbOptions);
        _repository = new PayslipRepository(_context);
    }

    private readonly Payslip _testPayslip = new()
    {
        PayslipId = Guid.NewGuid(),
        EmployeeId = "7734021",
        EmployeeName = "Hassan Patel",
        PayslipStartDate = new DateOnly(2025, 7, 1),
        PayslipEndDate = new DateOnly(2025, 7, 31),
        CompanyName = "Vertex Innovations Ltd",
        EmployeeDepartment = "Product Development",
        DaysWorkedPerWeek = 5,
        HoursWorked = 168,
        HolidayHours = 10,
        SickDates = [new DateOnly(2025, 7, 11)],
        GrossPay = 3931.20m,
        TaxAmountPaid = 388.20m,
        EmployeePensionAmountPaid = 157.25m,
        EmployerPensionAmountPaid = 157.25m,
        EmployeeUnionAmountPaid = 0m,
        NationalInsuranceAmountPaid = 313.50m,
        NetPay = 3155.00m
    };

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
