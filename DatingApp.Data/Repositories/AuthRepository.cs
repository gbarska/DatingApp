using System;
using System.Threading.Tasks;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using DatingApp.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == username);
            
            if(user == null)
                return null;
                
            // if (!AuthService.VerifyPasswordHash(password, user.PasswordHash, user.PassswordSalt))
            // return null;

            return user;
        }

        

        public async Task<User> Register(User user, string password)
        {
           var listaRetorno = AuthService.CreatePasswordHash(password);

            // user.PassswordSalt = listaRetorno[0];
            // user.PasswordHash = listaRetorno[1];

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;     
        } 

        public async Task<bool> UserExists(string username)
        {
             if (await _context.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
        }
    }
}