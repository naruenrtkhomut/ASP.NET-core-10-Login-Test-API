using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Models.Data
{
    public class JWTGenerator
    {
        public string? data { get; set; }
        public JWTGenerator(string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Models.Config.JWTSecretKey));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken newToken = new JwtSecurityToken(
                issuer: Models.Config.JWTIssuer,
                audience: Models.Config.JWTAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            data = new JwtSecurityTokenHandler().WriteToken(newToken);
        }
    }
}
