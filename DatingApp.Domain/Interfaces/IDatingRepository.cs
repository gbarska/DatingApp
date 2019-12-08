using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.Domain.Models;
using DatingApp.Domain.Shared;

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
         Task<Photo> GetPhoto(int Id);
         Task<Photo> GetMainPhotoForUser(int id);
         Task<Message> GetMessage(int id);
         Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessagesThread(int userId, int recipientId);
    }
}