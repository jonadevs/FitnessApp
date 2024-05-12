using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FitnessApp.API.DTOs;
using FitnessApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FitnessApp.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>
        {
            new("sub", user.UserId.ToString()),
            new("given_name", user.FirstName),
            new("family_name", user.LastName),
            new("date_of_birth", user.DateOfBirth.ToString() ?? string.Empty),
            new("city", user.City)
        };
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(new AuthenticationResultDTO { Token = tokenToReturn });
    }

    private static FitnessAppUser? ValidateUserCredentials(string? userName, string? password)
    {
        // We don't have a user DB or table. If you have, check the passed-through username/password against what's stored in the database.
        // For demo purposes we assume the credentials are valid.
        // return a new CityInfoUser (values would normally come from your user DB/table)
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        return new FitnessAppUser(1, userName, "Max", "Mustermann", new DateOnly(2010, 12, 31), "Berlin");
    }
}
