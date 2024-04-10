using IceCreamMAUI.Shared.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IceCreamMAUI.Api.Services;

public class TokenService(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) =>
        new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT: Issuer"],
            IssuerSigningKey = GetSymmetricSecurityKey(configuration),
        };

    public string GenerateJWT(LoggedInUser user)
    {
        var securityKey = GetSymmetricSecurityKey(_configuration);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var issuer = _configuration["JWT:Issuer"];
        var expireInMinutes = Convert.ToInt32(_configuration["JWT:ExpireInMinute"]);

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.StreetAddress, user.Address),
            ];

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: "*",
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireInMinutes),
            signingCredentials: credentials
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;

    }

    private static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
    {
        var secretKey = configuration["JWT:SecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        return securityKey;
    }
}
