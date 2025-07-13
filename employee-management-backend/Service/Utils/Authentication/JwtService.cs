using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using employee_management_backend.Service.Utils.Authentication.Interface;
using Microsoft.IdentityModel.Tokens;

namespace employee_management_backend.Service.Utils.Authentication;

public class JwtService(IConfiguration config) : IJwtService
{
    public string GenerateJwtToken(string employeeId, string employeeName)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, employeeId),
            new Claim("name", employeeName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(config["Jwt:ExpireMinutes"] ?? string.Empty)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
