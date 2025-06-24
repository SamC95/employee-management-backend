namespace employee_management_backend.Service.Utils.Passwords;

public static class HashPasswordUtil
{
    public static string PerformPasswordHash(string plainTextPassword)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        
        return hashedPassword;
    }

    public static bool VerifyPasswordHash(string plainTextPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
    }
}
