using System.Collections.Generic;
using System.Linq;
using DatingApp.Domain.Models;
using DatingApp.Domain.Services;
using Newtonsoft.Json;

namespace DatingApp.Data
{
    public class Seed
    {
        public static void SeedUsers(AppDbContext context)
        {
            if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("../DatingApp.Data/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;

                   var listaRetorno = AuthService.CreatePasswordHash("password");

                    passwordSalt = listaRetorno[0];
                    passwordHash = listaRetorno[1];

                    user.Username = user.Username.ToLower();
                    user.PassswordSalt = passwordSalt;
                    user.PasswordHash = passwordHash;
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}