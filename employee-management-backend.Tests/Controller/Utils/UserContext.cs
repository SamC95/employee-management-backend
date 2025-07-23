using System.Security.Claims;
namespace employee_management_backend.Tests.Controller.Utils;

public static class UserContext
{
    public static ClaimsPrincipal SetUserContext(string userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        
        return new ClaimsPrincipal(identity);
    }
}
