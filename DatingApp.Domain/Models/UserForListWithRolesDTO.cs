using System.Collections.Generic;

namespace DatingApp.Domain.Models
{
    public class UserForListWithRolesDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}