using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FitnessApp.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // we won't use this outside of their class, so we can scope it to this namespace

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }

            public string? Password { get; set; }
        }

        private class FitnessAppUser
        {
            public int UserId { get; set; }

            public string UserName { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public DateOnly? DateOfBirth { get; set; }

            public string City { get; set; }

            public FitnessAppUser(int userId, string userName, string firstName, string lastName, DateOnly? dateOfBirth, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate the username/password
            var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            // Step 2: create a token
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // The claims that
            var claimsForToken = new List<Claim>
            {
                new Claim("sub", user.UserId.ToString()),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("date_of_birth", user.DateOfBirth.ToString() ?? string.Empty),
                new Claim("city", user.City)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private static FitnessAppUser ValidateUserCredentials(string? userName, string? password)
        {
            // We don't have a user DB or table. If you have, check the passed-through username/password against what's stored in the database.
            // For demo purposes we assume the credentials are valid.
            // return a new CityInfoUser (values would normally come from your user DB/table)

            return new FitnessAppUser(1, userName ?? "", "Max", "Mustermann", new DateOnly(2010, 12, 31), "Berlin");
        }
    }
}
