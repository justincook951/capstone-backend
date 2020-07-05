using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CapstoneQuizAPI.Utilities
{
    public class JwtManager
    {
        private IConfiguration _config;
        public JwtManager(IConfiguration iconfig) { _config = iconfig; }
        public string GenerateToken(string username, bool isAdmin, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(_config.GetValue<string>("SecretKey"));
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var claimArray = new List<Claim>();
            claimArray.Add(new Claim(ClaimTypes.Name, username));
            if (isAdmin) {
                claimArray.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "admin")
                }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

    }
}
