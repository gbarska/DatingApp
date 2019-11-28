using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Services;
using DatingApp.Domain.DTOs;
using System.Linq;

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
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);

            users = users.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge -1);
                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

                users = users.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
             }

            if (userParams.Likees){
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likers){
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }
            
            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.CreatedAt);
                    break;
                    default: 
                        users = users.OrderByDescending(u => u.LastActive);
                    break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int userId, bool likers)
        {
               var user = await _context.Users
               .Include(x => x.Likers)
               .Include(u => u.Likees)
               .FirstOrDefaultAsync(u => u.Id == userId);

            if(likers)
            {
                return user.Likers.Where(u => u.LikeeId == userId ).Select(i => i.LikerId);
            } 
            else
            {
                return user.Likees.Where(u => u.LikerId == userId ).Select(i => i.LikeeId);
            }    
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public async Task<Photo> GetPhoto(int id)
        {
          var photo = await _context.Photos.FirstOrDefaultAsync( x => x.Id == id);
          return photo;
        }
    }
}