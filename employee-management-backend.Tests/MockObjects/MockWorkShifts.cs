using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockWorkShifts
{
    internal static WorkShift TestShift => new()
    {
        ShiftId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        StartTime = new TimeOnly(08, 30, 00),
        EndTime = new TimeOnly(17, 30, 00)
    };
}
