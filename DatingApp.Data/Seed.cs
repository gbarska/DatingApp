using System.Collections.Generic;
using System.Linq;
using DatingApp.Domain.Models;
using DatingApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.Data
{
    public class Seed
    {
        // public static void SeedUsers(AppDbContext context)
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if(!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("../DatingApp.Data/UserSeedData.json");

                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }
                
                foreach (var user in users)
                {
                //     byte[] passwordHash, passwordSalt;

                //    var listaRetorno = AuthService.CreatePasswordHash("password");

                //     passwordSalt = listaRetorno[0];
                //     passwordHash = listaRetorno[1];

                //     user.UserName = user.UserName.ToLower();
                //     // user.PassswordSalt = passwordSalt;
                //     // user.PasswordHash = passwordHash;
                //     context.Users.Add(user);

                userManager.CreateAsync(user, "password").Wait();
                userManager.AddToRoleAsync(user, "Member").Wait();
                }

                //create admin user
                var adminUser = new User 
                {
                    UserName = "Admin"
                };
                
                var result = userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;

                    userManager.AddToRolesAsync(admin, new [] {"Admin", "Moderator"}).Wait();
                }
                // context.SaveChanges();
            }
        }
    }
}