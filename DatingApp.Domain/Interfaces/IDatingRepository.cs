using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Domain.Models;
using DatingApp.Domain.Services;
using DatingApp.Domain.DTOs;

namespace  DatingApp.Domain.Interfaces
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         Task<Like> GetLike(int userId, int recipientId);
    }
}