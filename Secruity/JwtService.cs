using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace LabWeb.Secruity
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string GenerateToken(string Account, string Role)
        {
            JwtObject jwtObject = new JwtObject
            {
                Account = Account,
                Role = Role,
                Expire = DateTime.Now.AddMinutes(Convert.ToInt32(_config["AppSettings:ExpireTime"])).ToString()
            };

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,Account)
            };
            
            string[] roles = jwtObject.Role.Split(',');
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:SecretKey").Value!));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
            
            var token = new JwtSecurityToken(
                claims : claims,
                expires : DateTime.Now.AddHours(1),
                signingCredentials : creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string DecodeToken(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:SecretKey").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(jwt, validationParameters, out var validatedToken);
                var account = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                return account;
            }
            catch (SecurityTokenException)
            {
                // Token 無效
                return null;
            }
            catch (Exception)
            {
                // 解碼過程中發生錯誤
                return null;
            }
        }

    }
}