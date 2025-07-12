using employee_management_backend.Model;

namespace employee_management_backend.Tests.MockObjects;

public static class MockLogins
{
    internal static LoginDetails TestLogin => new()
    {
        UserId = "7734021",
        Password = "testpassword",
    };
}