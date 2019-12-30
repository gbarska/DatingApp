using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Shared;
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
        public async Task<User> GetUserWithUnapprovedPhotos(int id)
        {
            var user = await _context.Users
            .Include(y => y.UserRoles)
            .Include(x => x.Photos).IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<IEnumerable<User>> GetUsersWithUnapprovedPhotos(){
             var users = await _context.Users.Include(x => x.Photos).IgnoreQueryFilters().ToListAsync();
             
            return users;
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
          var photo = await _context.Photos.IgnoreQueryFilters().FirstOrDefaultAsync( x => x.Id == id);
          return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int id)
        {
          var photo = await _context.Photos.Where( x => x.UserId == id).FirstOrDefaultAsync(p => p.IsMain);
          return photo;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include( u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(u => u.RecipientId == userId && u.RecipientDeleted == false 
                     && u.SenderId == recipientId
                    || u.RecipientId == recipientId && u.SenderId == userId
                    && u.SenderDeleted == false)
            .OrderByDescending(m => m.MessageSentAt)
            .ToListAsync();

            return messages;
         }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = _context.Messages
            .Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include( u => u.Recipient).ThenInclude(p => p.Photos)
            .AsQueryable();

            switch(messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId && u.SenderDeleted == false);
                break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                    && u.RecipientDeleted == false  && u.IsRead == false);
                break;
            }


            messages = messages.OrderByDescending(d => d.MessageSentAt);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<UserForListWithRolesDTO>> GetUsersWithRoles()
        {
            var users = await _context.Users
            .OrderBy(x => x.UserName)
            .Select(user => new UserForListWithRolesDTO {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (from userRole in user.UserRoles
                 join role in _context.Roles 
                 on userRole.RoleId
                 equals role.Id
                 select role.Name).ToList()
            }).ToListAsync();

            return users;
        }
    }
}