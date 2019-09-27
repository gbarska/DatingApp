using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Services;

namespace DatingApp.Data.Repositories
{
    public class DatingRepository : IDatingRepository
    {
        private readonly AppDbContext _context;

        public DatingRepository(AppDbContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
           return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos);
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

      
    }
}