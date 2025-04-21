using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;

using System;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

namespace NewHospitalManagementSystem.Services

{

    public class JwtService

    {

        private readonly string _key;

        private readonly string _issuer;

        private readonly string _audience;

        private readonly int _expiryMinutes;

        public JwtService(IConfiguration config)

        {

            _key = config["JwtSettings:Key"];

            _issuer = config["JwtSettings:Issuer"];

            _audience = config["JwtSettings:Audience"];

            _expiryMinutes = int.Parse(config["JwtSettings:ExpiryMinutes"]);

        }

        public string GenerateToken(string username, string role)

        {

            var claims = new[]

            {

            new Claim(ClaimTypes.Name, username),

            new Claim(ClaimTypes.Role, role),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiry = DateTime.UtcNow.AddMinutes(_expiryMinutes);

            var token = new JwtSecurityToken(

                _issuer,

                _audience,

                claims,

                expires: expiry,

                signingCredentials: creds

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }

}

