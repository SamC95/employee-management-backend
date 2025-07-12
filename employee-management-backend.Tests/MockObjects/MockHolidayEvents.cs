using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockHolidayEvents
{
    public static HolidayEvent InProgressHolidayEvent => new()
    {
        EventId = Guid.NewGuid(),
        EmployeeId = "12345678",
        Date = new DateOnly(2018, 5, 5),
        Status = "in-progress",
    };
}