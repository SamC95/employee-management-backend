namespace employee_management_backend.Service.Utils;

public static class CreateRandomPassword
{
    // Creates a temporary password that the user will be prompted to change upon first login
    public static string GenerateRandomPassword(int length)
    {
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        var random = new Random();
        
        var chars = new char[length];

        for (var i = 0; i < length; i++)
        {
            chars[i] = validChars[random.Next(0, validChars.Length)];
        }
        
        return new string(chars);
    }
}