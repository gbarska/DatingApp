using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.API.Helpers
{
    public class ChatHub : Hub
    {
       private static Hashtable htUsers_ConIds = new Hashtable(20);
        public void registerConId(string userId)
        {
            if(htUsers_ConIds.ContainsKey(userId))
                htUsers_ConIds[userId] = Context.ConnectionId;
            else
                htUsers_ConIds.Add(userId, Context.ConnectionId);
        }
        public void Send(string userId, string message)
        {
            var connID = (string)htUsers_ConIds[userId];
             Clients.Client(connID).SendAsync(message);
        }

    }
}