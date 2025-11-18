using Microsoft.IdentityModel.Tokens;
using S3k.Utilitario.Models;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace S3k.Utilitario
{
    public class JwtUtil
    {
        public static string GenerateToken(UserClaim userClaim)
        {
            // variables
            string secretKey = ConfigurationManager.AppSettings["JWT_SecretKey"].ToString();
            string issuer = ConfigurationManager.AppSettings["JWT_Issuer"].ToString();
            string audience = ConfigurationManager.AppSettings["JWT_Audience"].ToString();

            if (!int.TryParse(ConfigurationManager.AppSettings["JWT_Expires"], out int expires))
            {
                expires = 24;
            }

            // header
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // claims
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("UserId", userClaim.UserId.ToString()),
                new Claim("UserName", userClaim.UserName ?? "unknown")
            });

            // payload
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(expires),
                SigningCredentials = signingCredentials
            };

            // token
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = securityTokenHandler.CreateToken(tokenDescriptor);

            string token = securityTokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
