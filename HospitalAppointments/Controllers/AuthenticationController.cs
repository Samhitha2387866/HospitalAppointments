using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

using NewHospitalManagementSystem.Models;

using System;

using System.IdentityModel.Tokens.Jwt;

using System.Linq;

using System.Security.Claims;

using System.Security.Cryptography;

using System.Text;

namespace NewHospitalManagementSystem.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class AuthenticationController : ControllerBase

    {

        private readonly ApplicationDBContext _context;

        private readonly IConfiguration _configuration;

        public AuthenticationController(ApplicationDBContext context, IConfiguration configuration)

        {

            _context = context;

            _configuration = configuration;

        }

        [HttpPost("login")]

        public ActionResult Login([FromBody] LoginRequest loginRequest)

        {

            var hashedPassword = HashPassword(loginRequest.Password);

            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == loginRequest.Email && d.PasswordHash == hashedPassword);

            var patient = _context.Patients.FirstOrDefault(p => p.Email == loginRequest.Email && p.PasswordHash == hashedPassword);

            if (doctor == null && patient == null)

            {

                return Unauthorized("Invalid email or password.");

            }

            // Assign role based on user type

            var role = doctor != null ? "Doctor" : "Patient";

            // Generate JWT Token

            var token = GenerateToken(loginRequest.Email, role);

            return Ok(new { Message = "Login successful", Token = token });

        }

        private string GenerateToken(string email, string role)

        {

            var claims = new[]

            {

                new Claim(ClaimTypes.Name, email),

                new Claim(JwtRegisteredClaimNames.Sub, email),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(ClaimTypes.Role, role) // Assign role

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(

                issuer: _configuration["JwtSettings:Issuer"],

                audience: _configuration["JwtSettings:Audience"],

                claims: claims,

                expires: DateTime.UtcNow.AddHours(1),

                signingCredentials: credentials

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private string HashPassword(string password)

        {

            using (var sha256 = SHA256.Create())

            {

                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                var builder = new StringBuilder();

                foreach (var b in bytes)

                {

                    builder.Append(b.ToString("x2"));

                }

                return builder.ToString();

            }

        }

    }

    public class LoginRequest

    {

        public string Email { get; set; }

        public string Password { get; set; }

    }

}
