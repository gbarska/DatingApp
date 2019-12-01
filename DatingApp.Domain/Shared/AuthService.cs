using System;
using System.Text;
using System.Security.Claims;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DatingApp.Domain.Models;
using System.Collections.Generic;

namespace DatingApp.Domain.Shared
{
    public static class AuthService
    {
        public static SecurityToken Generate(User user, IConfiguration config)
        {
            
            var claims = new[]
          {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public static string Write(SecurityToken token)
        {
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i =0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

         public static List<byte[]> CreatePasswordHash(string password)
        {
            var lista = new List<byte[]>();

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                lista.Add(hmac.Key);
                lista.Add(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            }
            
            return lista;
        }
    }
}